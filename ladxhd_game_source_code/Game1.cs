﻿using System;
﻿using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZ.Base;
using ProjectZ.Base.UI;
using ProjectZ.Editor;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.GameObjects;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Pages;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Screens;
using ProjectZ.InGame.Things;
using GBSPlayer;

#if WINDOWS
using Forms = System.Windows.Forms;
#endif

namespace ProjectZ
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static UiManager UiManager = new UiManager();
        public static ScreenManager ScreenManager = new ScreenManager();
        public static PageManager UiPageManager = new PageManager();
        public static Language LanguageManager = new Language();
        public static GameManager GameManager = new GameManager();
        public static GbsPlayer GbsPlayer = new GbsPlayer();
        public static Random RandomNumber = new Random();
        public static EditorManager EditorManager;
  
        public static int WindowWidth;
        public static int WindowHeight;
        public static int WindowWidthEnd;
        public static int WindowHeightEnd;
        public static int RenderWidth;
        public static int RenderHeight;

        private static int _lastWindowWidth;
        private static int _lastWindowHeight;
        private static bool _isFullscreen;
        private static bool _isResizing;
        private static bool _userExclusiveSetting;
        private static bool _wasMinimized;

        private static System.Drawing.Rectangle _lastWindowBounds;
        private static System.Drawing.Rectangle _lastWindowRestoreBounds;

        public static bool FpsSettingChanged;
        private readonly SimpleFps _fpsCounter = new SimpleFps();

        public static double FreezeTime;
        public static float TimeMultiplier;
        public static float DeltaTime;
        public static double TotalTime;
        public static double TotalGameTime;
        public static double TotalGameTimeLast;

        private static DoubleAverage _avgTotalMs = new DoubleAverage(30);
        private static DoubleAverage _avgTimeMult = new DoubleAverage(30);

        public static RenderTarget2D MainRenderTarget;
        private static RenderTarget2D _renderTarget1;
        private static RenderTarget2D _renderTarget2;
        private static bool _initRenderTargets;
        public static int ScreenScale;
        public static int UiScale;
        public static int UiRtScale;
        private static float gameScale;
        public static bool ScaleChanged;

        public static bool WasActive;
        public static bool UpdateGame;
        public static bool ForceDialogUpdate;
        public static bool EditorMode;
        public static bool SaveAndExitGame;
        public static bool AutoLoadSave;
        public static int AutoLoadSlot;

        private static bool _finishedLoading;

        public static string DebugText;
        public static float DebugTimeScale = 1.0f;
        public static bool DebugStepper;
        public static int DebugLightMode;
        public static int DebugBoxMode;
        public static bool DebugMode;
        public static bool ShowDebugText;
        private Vector2 _debugTextSize;

        public static bool FinishedLoading => _finishedLoading;

        public static Matrix GetMatrix => Matrix.CreateScale(new Vector3(
            (float)Graphics.PreferredBackBufferWidth / WindowWidth,
            (float)Graphics.PreferredBackBufferHeight / WindowHeight, 0));

#if WINDOWS
        private static Forms.Form _windowForm;
        private static Forms.FormWindowState _lastWindowState;
#endif

        public Game1(bool editorMode, bool loadSave, int loadSlot)
        {
#if WINDOWS
            _windowForm = (Forms.Form)Forms.Control.FromHandle(Window.Handle);
            _windowForm.Icon = Properties.Resources.Icon;

            // set the min size of the game
            // not sure why you can not simply set the min size of the client size directly...
            var deltaWidth = _windowForm.Width - _windowForm.ClientSize.Width;
            var deltaHeight = _windowForm.Height - _windowForm.ClientSize.Height;
            _windowForm.MinimumSize = new System.Drawing.Size(Values.MinWidth + deltaWidth, Values.MinHeight + deltaHeight);
#endif
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferredBackBufferWidth = 1500;
            Graphics.PreferredBackBufferHeight = 1000;

            Window.AllowUserResizing = true;
            IsMouseVisible = editorMode;

            EditorMode = editorMode;
            AutoLoadSave = loadSave;
            AutoLoadSlot = loadSlot;
        }

        protected override void Initialize()
        {
            EditorManager = new EditorManager(this);
            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            GbsPlayer.OnExit();

            base.OnExiting(sender, args);
        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            // Recreate game render targets
            GameManager?.UpdateRenderTargets();

            // Recreate main render targets in Game1
            UpdateRenderTargetSizes(WindowWidth, WindowHeight);
        }

        protected override void LoadContent()
        {
            // game control stuff
            ControlHandler.Initialize();

            // load game settings
            SettingsSaveLoad.LoadSettings();

            // Sound effects volume will not apply without at least one forced call of this.
            GameManager.UpdateSoundEffects();

            // init gbs player; load gbs file
            GbsPlayer.LoadFile(Path.Combine(Values.PathContentFolder, "Music", "awakening.gbs"));
            GbsPlayer.StartThread();

            // start loading the resources that are needed after the intro
            ThreadPool.QueueUserWorkItem(LoadContentThreaded);

            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Input Handler
            Components.Add(new InputHandler(this));

            // load the intro screen + the resources needed for it
            Resources.LoadIntro(Graphics.GraphicsDevice, Content);
            ScreenManager.LoadIntro(Content);

            // NOTE: NEEDED FOR EXCLUSIVE FULLSCREEN TO NOT CRASH
            UpdateScale(true);

            // toggle fullscreen
            if (GameSettings.IsFullscreen)
            {
                GameSettings.IsFullscreen = false;
                ToggleFullscreen();
            }
            // set the fps settings of the game
            UpdateFpsSettings();

            // Add extra HP if the user set it.
            ObjLives.Initialize();

#if WINDOWS
            _windowForm.ResizeBegin += OnResizeBegin;
            _windowForm.Resize += OnResize;
            _windowForm.ResizeEnd += OnResizeEnd;
#endif
            // Hook device reset
            GraphicsDevice.DeviceReset += OnDeviceReset;
        }

        private void LoadContentThreaded(Object obj)
        {
            // load resources
            Resources.LoadTextures(Graphics.GraphicsDevice, Content);
            Resources.LoadSounds(Content);

            GameManager.Load(Content);

            GameObjectTemplates.SetUpGameObjects();

            ScreenManager.Load(Content);

            // load the language files
            LanguageManager.Load();

            UiPageManager.Load(Content);

            if (EditorMode)
                EditorManager.SetUpEditorUi();

            _finishedLoading = true;

            // Now that everything has been loaded in, make sure the proper language textures are reloaded.
            Resources.RefreshDynamicResources();
        }

        protected override void Update(GameTime gameTime)
        {
            // Store the state to be referenced by InputHandler.
            WasActive = IsActive;

            // Mute music and sound effects if user disabled on inactive window.
            GameManager.HandleInactiveWindow(IsActive);

            // SetTransparency _fpsCounter counter
            _fpsCounter.Update(gameTime);

            // toggle fullscreen
            if ((InputHandler.KeyDown(Keys.LeftAlt) || InputHandler.KeyDown(Keys.RightAlt)) && InputHandler.KeyPressed(Keys.Enter))
            {
                ToggleFullscreen();
                InputHandler.ResetInputState();
                SettingsSaveLoad.SaveSettings();
            }

            if (_finishedLoading && !_initRenderTargets)
            {
                _initRenderTargets = true;
                WindowWidth = 0;
                WindowHeightEnd = 0;
            }

            if (WindowWidth != Window.ClientBounds.Width ||
                WindowHeight != Window.ClientBounds.Height)
                OnResize();

            UpdateRenderTargets();

            if (FpsSettingChanged)
            {
                UpdateFpsSettings();
                FpsSettingChanged = false;
            }

            if (ScaleChanged)
                UpdateScale();

            ControlHandler.Update();

            if (EditorMode && InputHandler.KeyPressed(Values.DebugToggleDebugText))
                ShowDebugText = !ShowDebugText;

            if (!DebugStepper)
            {
                TimeMultiplier = gameTime.ElapsedGameTime.Ticks / 166667f * DebugTimeScale;
                TotalGameTimeLast = TotalGameTime;

                // limit the game time so that it slows down if the steps are bigger than they would be for 30fps
                // if the timesteps get too big it would be hard (wast of time) to make the logic still function 100% correctly
                if (TimeMultiplier > 2.0f)
                {
                    TimeMultiplier = 2.0f;
                    DeltaTime = (TimeMultiplier * 1000.0f) / 60.0f;
                    TotalTime += (TimeMultiplier * 1000.0) / 60.0;
                    DebugText += "\nLow Framerate";

                    if (UpdateGame)
                        TotalGameTime += (TimeMultiplier * 1000.0) / 60.0;
                }
                else
                {
                    DeltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds * DebugTimeScale;
                    TotalTime += gameTime.ElapsedGameTime.TotalMilliseconds * DebugTimeScale;
                    if (UpdateGame)
                        TotalGameTime += gameTime.ElapsedGameTime.TotalMilliseconds * DebugTimeScale;
                }
            }

            if (_finishedLoading)
            {
                if (EditorMode)
                {
                    // update the ui
                    // need to be at the first place to be able to block input from the screen
                    UiManager.Update();

                    EditorManager.EditorUpdate(gameTime);
                }
                UiManager.CurrentScreen = "";

                // update the game ui
                UiPageManager.Update(gameTime);
            }

            // update the screen manager
            UpdateGame = true;
            if (!DebugStepper || InputHandler.KeyPressed(Keys.M))
                ScreenManager.Update(gameTime);

            if (_finishedLoading)
            {
                DebugText += _fpsCounter.Msg;

                _avgTotalMs.AddValue(gameTime.ElapsedGameTime.TotalMilliseconds);
                _avgTimeMult.AddValue(TimeMultiplier);
                DebugText += $"\ntotal ms:      {_avgTotalMs.Average,6:N3}" +
                             $"\ntime mult:     {_avgTimeMult.Average,6:N3}" +
                             $"\ntime scale:    {DebugTimeScale}" +
                             $"\ntime:          {TotalGameTime}";

                DebugText += "\nHistory Enabled: " + GameManager.SaveManager.HistoryEnabled + "\n";
            }
            base.Update(gameTime);
        }

        private void EnsureRenderTargets()
        {
            if (MainRenderTarget == null || _renderTarget1 == null || _renderTarget2 == null)
            {
                UpdateRenderTargetSizes(Math.Max(1, WindowWidth), Math.Max(1, WindowHeight));
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            EnsureRenderTargets();

            if (!_finishedLoading)
            {
                ScreenManager.Draw(SpriteBatch);
                return;
            }
            _fpsCounter.CountDraw();

            ScreenManager.DrawRT(SpriteBatch);

            if (MainRenderTarget == null)
            {
                GraphicsDevice.Clear(Color.CadetBlue);
                ScreenManager.Draw(SpriteBatch);
                return;
            }
            Graphics.GraphicsDevice.SetRenderTarget(MainRenderTarget);
            GraphicsDevice.Clear(Color.CadetBlue);

            ScreenManager.Draw(SpriteBatch);

            BlurImage();
            {
                Graphics.GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

                //draw the original image
                SpriteBatch.Draw(MainRenderTarget, new Rectangle(0, 0, MainRenderTarget.Width, MainRenderTarget.Height), Color.White);

                SpriteBatch.End();
            }

            {
                if (_renderTarget2 != null)
                {
                    Resources.BlurEffect.Parameters["sprBlur"].SetValue(_renderTarget2);
                    Resources.RoundedCornerBlurEffect.Parameters["sprBlur"].SetValue(_renderTarget2);
                }
                SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, null, null, Resources.RoundedCornerBlurEffect, GetMatrix);

                // blurred ui parts
                UiManager.DrawBlur(SpriteBatch);

                // blured stuff
                GameManager.InGameOverlay.InGameHud.DrawBlur(SpriteBatch);

                // background for the debug text
                DebugTextBackground();

                SpriteBatch.End();
            }

            {
                // draw the top part
                SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, GetMatrix);

                // draw the ui part
                UiManager.Draw(SpriteBatch);

                // draw the game ui
                UiPageManager.Draw(SpriteBatch);

                // draw the screen tops
                ScreenManager.DrawTop(SpriteBatch);

                // draw the debug text
                DrawDebugText();
                DebugText = "";

#if DEBUG
                if (GameManager.SaveManager.HistoryEnabled)
                    SpriteBatch.Draw(Resources.SprWhite, new Rectangle(0, WindowHeight - 6, WindowWidth, 6), Color.Red);
#endif

                SpriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void BlurImage()
        {
            if (MainRenderTarget == null || _renderTarget1 == null || _renderTarget2 == null)
                return; // nothing to do safely

            var blurValue = 0.2f;

            // safe: _renderTarget1/_renderTarget2 are non-null and have positive width/height
            Resources.BlurEffectH.Parameters["pixelX"].SetValue(1.0f / _renderTarget1.Width);
            Resources.BlurEffectV.Parameters["pixelY"].SetValue(1.0f / _renderTarget1.Height);

            var mult0 = blurValue;
            var mult1 = (1 - blurValue * 2) / 2;
            Resources.BlurEffectH.Parameters["mult0"].SetValue(mult0);
            Resources.BlurEffectH.Parameters["mult1"].SetValue(mult1);
            Resources.BlurEffectV.Parameters["mult0"].SetValue(mult0);
            Resources.BlurEffectV.Parameters["mult1"].SetValue(mult1);

            // resize
            Graphics.GraphicsDevice.SetRenderTarget(_renderTarget2);
            SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, null, null, null, null);
            SpriteBatch.Draw(MainRenderTarget, new Rectangle(0, 0, _renderTarget2.Width, _renderTarget2.Height), Color.White);
            SpriteBatch.End();

            for (var i = 0; i < 2; i++)
            {
                // v blur
                Graphics.GraphicsDevice.SetRenderTarget(_renderTarget1);
                SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, null, null, Resources.BlurEffectV, null);
                SpriteBatch.Draw(_renderTarget2, Vector2.Zero, Color.White);
                SpriteBatch.End();

                // h blur
                Graphics.GraphicsDevice.SetRenderTarget(_renderTarget2);
                SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, null, null, Resources.BlurEffectH, null);
                SpriteBatch.Draw(_renderTarget1, Vector2.Zero, Color.White);
                SpriteBatch.End();
            }
        }

        public void TriggerFpsSettings()
        {
            if (!IsFixedTimeStep)
            {
                IsFixedTimeStep = true;
                Graphics.SynchronizeWithVerticalRetrace = false;
            }
            else
            {
                IsFixedTimeStep = false;
                Graphics.SynchronizeWithVerticalRetrace = true;
            }
            Graphics.ApplyChanges();
        }

        public static void SwitchFullscreenWindowedSetting()
        {
            _userExclusiveSetting = GameSettings.ExFullscreen;

            if (GameSettings.ExFullscreen && Graphics.IsFullScreen ||
                GameSettings.ExFullscreen && _isFullscreen)
            {
                ToggleFullscreen();
                GameSettings.ExFullscreen = !GameSettings.ExFullscreen;
                ToggleFullscreen();
            }
            else
            {
                GameSettings.ExFullscreen = !GameSettings.ExFullscreen;
            }
            GameSettings.ExFullscreen = !_userExclusiveSetting;
        }

        public static void ToggleFullscreen()
        {
            GameSettings.IsFullscreen = !GameSettings.IsFullscreen;

            var screenBounds = System.Windows.Forms.Screen.GetBounds(_windowForm);

            if (GameSettings.ExFullscreen)
            {
                if (!Graphics.IsFullScreen)
                {
                    _lastWindowWidth = Graphics.PreferredBackBufferWidth;
                    _lastWindowHeight = Graphics.PreferredBackBufferHeight;

                    _lastWindowRestoreBounds = _windowForm.RestoreBounds;

                    Graphics.PreferredBackBufferWidth = screenBounds.Width;
                    Graphics.PreferredBackBufferHeight = screenBounds.Height;

                    _lastWindowState = _windowForm.WindowState;
                }
                else
                {
                    if (_lastWindowState != Forms.FormWindowState.Maximized)
                    {
                        Graphics.PreferredBackBufferWidth = _lastWindowWidth;
                        Graphics.PreferredBackBufferHeight = _lastWindowHeight;
                    }
                }
                Graphics.ToggleFullScreen();

                if (_lastWindowState == Forms.FormWindowState.Maximized)
                {
                    // restore the window size of the normal sized window
                    _windowForm.Bounds = _lastWindowRestoreBounds;

                    _windowForm.WindowState = _lastWindowState;
                }
            }
            else
            {
                _isFullscreen = !_isFullscreen;
            
                // change to fullscreen
                if (_isFullscreen)
                {
                    _lastWindowState = _windowForm.WindowState;
                    _lastWindowBounds = _windowForm.Bounds;

                    _windowForm.FormBorderStyle = Forms.FormBorderStyle.None;
                    _windowForm.WindowState = Forms.FormWindowState.Normal;
                    _windowForm.Bounds = screenBounds;
                }
                else
                {
                    _windowForm.FormBorderStyle = Forms.FormBorderStyle.Sizable;

                    if (_lastWindowState == Forms.FormWindowState.Maximized)
                    {
                        // this is set to not loose the old state because fullscreen and windowed are both using the "Normal" state
                        _windowForm.Bounds = _lastWindowRestoreBounds;

                        _windowForm.WindowState = _lastWindowState;
                    }
                    else
                    {
                        _windowForm.WindowState = _lastWindowState;
                        _windowForm.Bounds = _lastWindowBounds;
                    }
                }
            }
            GameManager?.UpdateRenderTargets();
        }

        private void ClientSizeChanged(object sender, EventArgs e)
        {
            OnResize();
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        }
        
        public void DebugTextBackground()
        {
            if (!ShowDebugText)
                return;

            _debugTextSize = Resources.GameFont.MeasureString(DebugText);

            // draw the background
            SpriteBatch.Draw(_renderTarget2, new Rectangle(0, 0,
                (int)(_debugTextSize.X * 2) + 20, (int)(_debugTextSize.Y * 2) + 20), Color.White);
        }

        public void DrawDebugText()
        {
            if (!ShowDebugText)
                return;

            SpriteBatch.Draw(Resources.SprWhite, new Rectangle(0, 0,
                    (int)(_debugTextSize.X * 2) + 20, (int)(_debugTextSize.Y * 2) + 20), Color.Black * 0.75f);

            SpriteBatch.DrawString(Resources.GameFont, DebugText, new Vector2(10), Color.White,
                0, Vector2.Zero, new Vector2(2f), SpriteEffects.None, 0);
        }

        public void UpdateFpsSettings()
        {
            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = GameSettings.VerticalSync;
            Graphics.ApplyChanges();
        }

        private void OnResizeBegin(object sender, EventArgs e)
        {
            _isResizing = true;
        }

        private void OnResize(object sender, EventArgs e)
        {
#if WINDOWS
            // save the restore bounds when going into borderless fullscreen mode from an maximized state
            if (_isFullscreen && _windowForm.WindowState == Forms.FormWindowState.Maximized)
                _lastWindowRestoreBounds = _windowForm.RestoreBounds;

            // minimize the fullscreen window
            if (GameSettings.ExFullscreen && Graphics.IsFullScreen && _windowForm.WindowState == Forms.FormWindowState.Minimized && !_wasMinimized)
            {
                _wasMinimized = true;

                Graphics.ToggleFullScreen();
                _windowForm.WindowState = Forms.FormWindowState.Minimized;
            }
            // reopen the fullscreen window
            if (GameSettings.ExFullscreen && _windowForm.WindowState == Forms.FormWindowState.Normal && _wasMinimized)
            {
                _wasMinimized = false;
                ToggleFullscreen();
            }
#endif
        }

        private void OnResizeEnd(object sender, EventArgs e)
        {
            _isResizing = false;
        }

        private void OnResize()
        {
            // if minimized window bounds may be zero; ignore those
            if (Window.ClientBounds.Width <= 0 || Window.ClientBounds.Height <= 0)
                return;

            WindowWidth = Window.ClientBounds.Width;
            WindowHeight = Window.ClientBounds.Height;
            UpdateScale();
        }

        private void UpdateScale(bool SkipEditor = false)
        {
            // Scale of the game field.
            ScreenScale = MathHelper.Clamp(Math.Min(WindowWidth / Values.MinWidth, WindowHeight / Values.MinHeight), 1, 25);

            // Calculate the game scale that is used for auto scaling.
            gameScale = MathHelper.Clamp(Math.Min(WindowWidth / (float)Values.MinWidth, WindowHeight / (float)Values.MinHeight), 1, 25);

            // If set to autoscale (11) used the calculated value. Otherwise use the value set by the user.
            MapManager.Camera.Scale = GameSettings.GameScale == 11 
                ? MathF.Ceiling(gameScale) 
                : GameSettings.GameScale;

            // The game scale must be at least 1x.
            if (MapManager.Camera.Scale < 1)
            {
                MapManager.Camera.Scale = 1 / (2 - MapManager.Camera.Scale);
                GameManager.SetGameScale(1);
            }
            // If it's 1x or greater. We use "gameScale" directly here as a float as it allows fractional 
            // values while manually setting the scale only allows upscaling using integer values.
            else
            {
                // If set to autoscale (11) use that. Otherwise use the value set by the user.
                float newGameScale = GameSettings.GameScale == 11 
                    ? gameScale 
                    : GameSettings.GameScale;

                GameManager.SetGameScale(newGameScale);
            }
            // Scale of the user interface.
            if (GameSettings.UiScale > ScreenScale)
            {
                UiScale = ScreenScale;
            }
            else
            {
                UiScale = GameSettings.UiScale == 0 ? ScreenScale : MathHelper.Clamp(GameSettings.UiScale, 1, ScreenScale);
            }
            // NOTE: This was used as a workaround to issues with Exclusive Fullscreen mode. Null render targets caused editor to crash on start up.
            if (!SkipEditor) UiManager.SizeChanged();

            // NOTE: I can't remember if UiPageManager actually needs a forced resize here. Might be more workarounds to null render targets in exclusive fullscreen.
            ScreenManager.OnResize(WindowWidth, WindowHeight);
            UiPageManager.OnResize(WindowWidth, WindowHeight);

            ScaleChanged = false;
        }

        private void UpdateRenderTargets()
        {
            if (_isResizing ||
                WindowWidthEnd == WindowWidth && WindowHeightEnd == WindowHeight)
                return;

            UiRtScale = UiScale;

            WindowWidthEnd = WindowWidth;
            WindowHeightEnd = WindowHeight;

            UpdateRenderTargetSizes(WindowWidth, WindowHeight);

            ScreenManager.OnResizeEnd(WindowWidth, WindowHeight);
        }

        private void UpdateRenderTargetSizes(int width, int height)
        {
            width = Math.Max(1, width);
            height = Math.Max(1, height);

            Resources.BlurEffect.Parameters["width"].SetValue(width);
            Resources.BlurEffect.Parameters["height"].SetValue(height);
            Resources.RoundedCornerBlurEffect.Parameters["textureWidth"].SetValue(width);
            Resources.RoundedCornerBlurEffect.Parameters["textureHeight"].SetValue(height);

            var blurScale = MathHelper.Clamp(MapManager.Camera.Scale / 2, 1, 10);
            var blurRtWidth = Math.Max(1, (int)(width / blurScale));
            var blurRtHeight = Math.Max(1, (int)(height / blurScale));

            RenderTarget2D newMain = null;
            RenderTarget2D newRt1 = null;
            RenderTarget2D newRt2 = null;

            try
            {
                newMain = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
                newRt1 = new RenderTarget2D(Graphics.GraphicsDevice, blurRtWidth, blurRtHeight);
                newRt2 = new RenderTarget2D(Graphics.GraphicsDevice, blurRtWidth, blurRtHeight);
            }
            catch (Exception ex)
            {
                newMain?.Dispose();
                newRt1?.Dispose();
                newRt2?.Dispose();
                return;
            }
            MainRenderTarget?.Dispose();
            _renderTarget1?.Dispose();
            _renderTarget2?.Dispose();

            MainRenderTarget = newMain;
            _renderTarget1 = newRt1;
            _renderTarget2 = newRt2;
        }
    }
}