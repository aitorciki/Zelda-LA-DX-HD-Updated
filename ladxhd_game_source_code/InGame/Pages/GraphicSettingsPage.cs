using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class GraphicSettingsPage : InterfacePage
    {
        private readonly InterfaceSlider _uiScaleSlider;
        private readonly InterfaceSlider _gameScaleSlider;
        private readonly InterfaceListLayout _bottomBar;
        private readonly InterfaceListLayout _toggleFullscreen;
        private readonly InterfaceListLayout _toggleExFullscreen;

        public GraphicSettingsPage(int width, int height)
        {
            // Graphics Settings Layout
            var _graphicSettingsLayout = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            var buttonWidth = 320;
            _graphicSettingsLayout.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_graphics_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            var contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Slider: Game Scale
            _gameScaleSlider = new InterfaceSlider(Resources.GameFont, "settings_graphics_game_scale",
                buttonWidth, new Point(1, 2), -3, 21, 1, GameSettings.GameScale, number =>
                {
                    GameSettings.GameScale = number;
                    Game1.ScaleChanged = true;
                })
            { SetString = number => GameScaleSliderAdjustmentString(number) };
            contentLayout.AddElement(_gameScaleSlider);

            // Slider: UI Scale
            _uiScaleSlider = new InterfaceSlider(Resources.GameFont, "settings_graphics_ui_scale",
                buttonWidth, new Point(1, 2), 1, 11, 1, GameSettings.UiScale-1, number =>
                {
                    GameSettings.UiScale = number;
                    Game1.ScaleChanged = true;
                })
            { SetString = number => UIScaleSliderAdjustmentString(number) };
            contentLayout.AddElement(_uiScaleSlider);

            // Button: Fullscreen
            _toggleFullscreen = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_graphics_fullscreen", GameSettings.IsFullscreen, newState => 
                {
                    Game1.ToggleFullscreen();
                    Game1.ScaleChanged = true;
                });
            contentLayout.AddElement(_toggleFullscreen);

            // Button: Exclusive Fullscreen
            _toggleExFullscreen = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_graphics_exfullscreen", GameSettings.ExFullscreen, newState => { GameSettings.ExFullscreen = newState; });
            contentLayout.AddElement(_toggleExFullscreen);

            // Button: Dynamic Shadows
            var shadowToggle = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
               "settings_graphics_shadow", GameSettings.EnableShadows, newState => GameSettings.EnableShadows = newState);
             contentLayout.AddElement(shadowToggle);

            // Button: Vertical Sync
            var toggleFpsLock = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_graphics_fps_lock", GameSettings.VerticalSync, newState =>
                {
                    GameSettings.VerticalSync = newState;
                    Game1.FpsSettingChanged = true;
                });
            contentLayout.AddElement(toggleFpsLock);

            // Button: Epilepsy Safe
            var toggleEpilepsySafe = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_graphics_epilepsysafe", GameSettings.EpilepsySafe, newState => { GameSettings.EpilepsySafe = newState; });
            contentLayout.AddElement(toggleEpilepsySafe);

            // Bottom Bar / Back Button:
            _bottomBar = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));
            _graphicSettingsLayout.AddElement(contentLayout);
            _graphicSettingsLayout.AddElement(_bottomBar);
            PageLayout = _graphicSettingsLayout;
        }

        public override void Update(CButtons pressedButtons, GameTime gameTime)
        {
            base.Update(pressedButtons, gameTime);

            UpdateFullscreenState();
            UpdateGameScaleSlider();

            // close the page
            if (ControlHandler.ButtonPressed(ControlHandler.CancelButton))
                Game1.UiPageManager.PopPage();
        }

        private string GameScaleSliderAdjustmentString(int number)
        {
            // Translate values below 1x and when autoscale is set.
            return number switch
            {
                 0 => " 50%",
                -1 => " 33%",
                -2 => " 25%",
                -3 => " 20%",
                21 => " Auto-Detect",
                _ => " " + number + "x"
            };
        }

        private string UIScaleSliderAdjustmentString(int number)
        {
            if (number == 11)
                return " Auto-Detect";
            return " " + number + "x";
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

        public override void OnResize(int newWidth, int newHeight)
        {

        }

        private void UpdateFullscreenState()
        {
            var toggle = (InterfaceToggle)_toggleFullscreen.Elements[1];
            if (toggle.ToggleState != GameSettings.IsFullscreen)
                toggle.SetToggle(GameSettings.IsFullscreen);
        }

        private void UpdateGameScaleSlider()
        {
            // The step starts at 0 and ends at max. Add the amount it goes negative.
            _gameScaleSlider.CurrentStep = GameSettings.GameScale + 3;
        }
    }
}
