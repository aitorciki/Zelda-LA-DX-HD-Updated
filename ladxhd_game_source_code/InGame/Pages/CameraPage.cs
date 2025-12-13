using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class CameraSettingsPage : InterfacePage
    {
        private readonly InterfaceListLayout _cameraOptionsList;
        private readonly InterfaceListLayout _contentLayout;
        private readonly InterfaceListLayout _bottomBar;
        private readonly InterfaceListLayout _toggleClassicCamera;
        private readonly InterfaceListLayout _toggleClassicDungeon;
        private readonly InterfaceListLayout _toggleCameraLock;
        private readonly InterfaceListLayout _toggleCameraSmooth;
        private readonly InterfaceSlider _sliderCameraBorder;
        private readonly InterfaceSlider _sliderBorderOpacity;
        public static bool _reloadMenus;
        private bool _showTooltip;

        public void SetClassicCamera(bool state) => ((InterfaceToggle)_toggleClassicCamera.Elements[1]).ToggleState = state;
        public void SetClassicDungeon(bool state) => ((InterfaceToggle)_toggleClassicDungeon.Elements[1]).ToggleState = state;
        public void SetClassicBorder(int value) => _sliderCameraBorder.CurrentStep = value;
        public void SetCameraLock(bool state) => ((InterfaceToggle)_toggleCameraLock.Elements[1]).ToggleState = state;
        public void SetCameraSmooth(bool state) => ((InterfaceToggle)_toggleCameraSmooth.Elements[1]).ToggleState = state;

        public CameraSettingsPage(int width, int height)
        {
            EnableTooltips = true;

            var buttonWidth = 320;
            var buttonHeight = 16;

            // Camera Settings Layout
            _cameraOptionsList = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            _cameraOptionsList.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_camera_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Button: Classic Camera
            _toggleClassicCamera = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_camera_classiccam", GameSettings.ClassicCamera, 
                newState => { GameSettings.ClassicCamera = newState; Game1.ScaleChanged = true; UpdateInterfaceColors(); });
            _contentLayout.AddElement(_toggleClassicCamera);

            // Button: Dungeons Only
            _toggleClassicDungeon = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_camera_classicdungeon", GameSettings.ClassicDungeon, 
                newState => { GameSettings.ClassicDungeon = newState; Game1.ScaleChanged = true; });
            _contentLayout.AddElement(_toggleClassicDungeon);

            // Slider: Camera Border
            _sliderCameraBorder = new InterfaceSlider(Resources.GameFont, "settings_camera_camborder",
                buttonWidth, new Point(1, 2), 0, 2, 1, GameSettings.ClassicBorders, 
                number => { GameSettings.ClassicBorders = number; Game1.ScaleChanged = true; }) 
                { SetString = number => ClassicBorderAdjustment(number) };
            _contentLayout.AddElement(_sliderCameraBorder);

            // Slider: Blackout Amount
            _sliderBorderOpacity = new InterfaceSlider(Resources.GameFont, "settings_camera_blackpercent",
                buttonWidth, new Point(1, 2), 0, 100, 5, (int)(GameSettings.ClassicAlpha * 100),
                number => { GameSettings.ClassicAlpha = (float)(number * 0.01); })
                { SetString = number => AddedMoveSpeedSliderAdjustment(number) };
            _contentLayout.AddElement(_sliderBorderOpacity);

            // Button: Camera Lock
            _toggleCameraLock = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_camera_cameralock", GameSettings.CameraLock, 
                newState => { GameSettings.CameraLock = newState; });
            _contentLayout.AddElement(_toggleCameraLock);

            // Button: Smooth Camera
            _toggleCameraSmooth = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_camera_smoothcamera", GameSettings.SmoothCamera, 
                newState => { GameSettings.SmoothCamera = newState; });
            _contentLayout.AddElement(_toggleCameraSmooth);

            // Button: Screen-Shake
            var toggleScreenShake = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_camera_screenshake", GameSettings.ScreenShake, 
                newState => { GameSettings.ScreenShake = newState; });
            _contentLayout.AddElement(toggleScreenShake);

            // Bottom Bar / Back Button:
            _bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));
            _cameraOptionsList.AddElement(_contentLayout);
            _cameraOptionsList.AddElement(_bottomBar);
            PageLayout = _cameraOptionsList;

            // Update button colors.
            UpdateInterfaceColors();
        }

        public override void Update(CButtons pressedButtons, GameTime gameTime)
        {
            base.Update(pressedButtons, gameTime);

            // The back button was pressed.
            if (ControlHandler.ButtonPressed(ControlHandler.CancelButton))
                Game1.UiPageManager.PopPage();

            // The tooltip button was pressed.
            if (ControlHandler.ButtonPressed(CButtons.Y))
            {
                _showTooltip = !_showTooltip;
                if (_showTooltip)
                    Game1.GameManager.PlaySoundEffect("D360-21-15");
            }
            // Hide the tooltip when pressing anything.
            else if (ControlHandler.AnyButtonPressed())
                _showTooltip = false;
        }

        public override void OnLoad(Dictionary<string, object> intent)
        {
            // the left button is always the first one selected
            _bottomBar.Deselect(false);
            _bottomBar.Select(InterfaceElement.Directions.Left, false);
            _bottomBar.Deselect(false);

            PageLayout.Deselect(false);
            PageLayout.Select(InterfaceElement.Directions.Top, false);
        }

        public void UpdateInterfaceColors()
        {
            // Toggling classic camera "grays out" some options depending on its state.
            _toggleClassicDungeon.ToggleElementColors(GameSettings.ClassicCamera);
            _sliderCameraBorder.ToggleSliderColors(GameSettings.ClassicCamera);
            _sliderBorderOpacity.ToggleSliderColors(GameSettings.ClassicCamera);
            _toggleCameraLock.ToggleElementColors(!GameSettings.ClassicCamera);
        }

        private string AddedMoveSpeedSliderAdjustment(int number)
        {
            return ": " + number + "%";
        }

        private string ClassicBorderAdjustment(int number)
        {
            return ": " + number switch
            {
                0 => Game1.LanguageManager.GetString("tooltip_camera_camborderA", "error"),
                1 => Game1.LanguageManager.GetString("tooltip_camera_camborderB", "error"),
                2 => Game1.LanguageManager.GetString("tooltip_camera_camborderC", "error")
            };
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int height, float alpha)
        {
            // Always draw the menu even when not showing tooltips.
            base.Draw(spriteBatch, position, height, alpha);

            // If the user pressed the top most face button, show the tooltip window.
            if (_showTooltip)
            {
                string tooltipText = GetOptionToolip();
                PageTooltip.Draw(spriteBatch, tooltipText);
            }
        }

        private string GetOptionToolip()
        {
            // Detect back button press by checking the index of the main InterfaceListLayout.
            if (_cameraOptionsList.SelectionIndex == 2)
                return  Game1.LanguageManager.GetString("tooltip_default", "error");

            // Detect the chosen button by checking the content InterfaceListLayout.
            int index = _contentLayout.SelectionIndex;
            string tooltip = "Select an option to view its tooltip.";

            // Use the selected index to determine which tooltip to show.
            switch (index) 
            {
                case 0:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_classiccam", "error"); break; }
                case 1:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_classicdungeon", "error"); break; }
                case 2:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_camborder", "error"); break; }
                case 3:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_blackpercent", "error"); break; }
                case 4:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_cameralock", "error"); break; }
                case 5:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_smoothcamera", "error"); break; }
                case 6:  { tooltip = Game1.LanguageManager.GetString("tooltip_camera_screenshake", "error"); break; }
            }
            // Display the tooltip in the tooltip window.
            return tooltip;
        }
    }
}