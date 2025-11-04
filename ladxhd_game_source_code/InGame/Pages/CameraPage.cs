using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class CameraSettingsPage : InterfacePage
    {
        private readonly InterfaceListLayout _contentLayout;
        private readonly InterfaceListLayout _bottomBar;
        private readonly InterfaceListLayout toggleClassicDungeon;
        private readonly InterfaceListLayout toggleCameraBorder;
        private readonly InterfaceListLayout toggleCameraLock;
        private readonly InterfaceSlider sliderBorderOpacity;
        public static bool _reloadMenus;

        public CameraSettingsPage(int width, int height)
        {
            var buttonWidth = 320;

            // Camera Settings Layout
            var cameraOptionsList = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            cameraOptionsList.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_camera_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Button: Classic Camera
            var toggleClassicCamera = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_classiccam", GameSettings.ClassicCamera, newState => { GameSettings.ClassicCamera = newState; Game1.ScaleChanged = true; UpdateInterfaceColors(); });
            _contentLayout.AddElement(toggleClassicCamera);

            // Button: Dungeons Only
            toggleClassicDungeon = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_classicdungeon", GameSettings.ClassicDungeon, newState => { GameSettings.ClassicDungeon = newState; Game1.ScaleChanged = true; });
            _contentLayout.AddElement(toggleClassicDungeon);

            // Button: Camera Border
            toggleCameraBorder = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_camborder", GameSettings.ClassicBorder, newState => { GameSettings.ClassicBorder = newState; });
            _contentLayout.AddElement(toggleCameraBorder);

            // Slider: Blackout Amount
            sliderBorderOpacity = new InterfaceSlider(Resources.GameFont, "settings_camera_blackpercent",
                buttonWidth, new Point(1, 2), 0, 100, 5, (int)(GameSettings.ClassicAlpha * 100),
                number =>
                {
                    GameSettings.ClassicAlpha = (float)(number * 0.01);
                })
            { SetString = number => AddedMoveSpeedSliderAdjustment(number) };
            _contentLayout.AddElement(sliderBorderOpacity);

            // Button: Camera Lock
            toggleCameraLock = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_cameralock", GameSettings.CameraLock, newState => { GameSettings.CameraLock = newState; });
            _contentLayout.AddElement(toggleCameraLock);

            // Button: Smooth Camera
            var smoothCameraToggle = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_smoothcamera", GameSettings.SmoothCamera, newState => { GameSettings.SmoothCamera = newState; });
            _contentLayout.AddElement(smoothCameraToggle);

            // Button: Screen-Shake
            var toggleScreenShake = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 14), new Point(5, 2),
                "settings_camera_screenshake", GameSettings.ScreenShake, newState => { GameSettings.ScreenShake = newState; });
            _contentLayout.AddElement(toggleScreenShake);

            // Bottom Bar / Back Button:
            _bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));
            cameraOptionsList.AddElement(_contentLayout);
            cameraOptionsList.AddElement(_bottomBar);
            PageLayout = cameraOptionsList;

            // Update button colors.
            UpdateInterfaceColors();
        }

        public override void Update(CButtons pressedButtons, GameTime gameTime)
        {
            base.Update(pressedButtons, gameTime);

            // close the page
            if (ControlHandler.ButtonPressed(ControlHandler.CancelButton))
                Game1.UiPageManager.PopPage();
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
            toggleClassicDungeon.ToggleElementColors(GameSettings.ClassicCamera);
            toggleCameraBorder.ToggleElementColors(GameSettings.ClassicCamera);
            sliderBorderOpacity.ToggleSliderColors(GameSettings.ClassicCamera);
            toggleCameraLock.ToggleElementColors(!GameSettings.ClassicCamera);
        }

        private string AddedMoveSpeedSliderAdjustment(int number)
        {
            return ": " + number + "%";
        }
    }
}