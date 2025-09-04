using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Screens;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class MiscellaneousPage : InterfacePage
    {
        private readonly ContentManager _contentManager;
        private readonly InterfaceListLayout _miscellaneousList;
        private readonly InterfaceListLayout _contentLayout;

        private readonly InterfaceListLayout _bottomBar;
        private readonly InterfaceListLayout _fontSelect;
        private readonly InterfaceListLayout _toggleMenuBricks;

        public static bool WasReload;

        public MiscellaneousPage(int width, int height, ContentManager content)
        {
            _contentManager = content;

            // graphic settings layout
            _miscellaneousList = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            var buttonWidth = 320;
            _miscellaneousList.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_misc_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Original Menu Border:
            _toggleMenuBricks = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_misc_menubricks", GameSettings.OldMenuBorder, newState => 
                { 
                    GameSettings.OldMenuBorder = newState;

                    var texture = newState
                        ? content.Load<Texture2D>("Menu/menuBackgroundAlt")
                        : content.Load<Texture2D>("Menu/menuBackground");

                    var menuScreen = (MenuScreen)Game1.ScreenManager.GetScreen(Values.ScreenNameMenu);
                    menuScreen?.SetBackground(texture);
                });
            _contentLayout.AddElement(_toggleMenuBricks);

            // Font Toggle:
            _fontSelect = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_misc_vwfont", GameSettings.VarWidthFont, newState => { PressButtonDialogFontChange(newState); });
            _contentLayout.AddElement(_fontSelect);

            _bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };

            // back button
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element =>
            {
                Game1.UiPageManager.PopPage();
            }));

            _miscellaneousList.AddElement(_contentLayout);
            _miscellaneousList.AddElement(_bottomBar);

            PageLayout = _miscellaneousList;
        }

        public override void Update(CButtons pressedButtons, GameTime gameTime)
        {
            base.Update(pressedButtons, gameTime);

            // close the page
            if (ControlHandler.ButtonPressed(CButtons.B))
                Game1.UiPageManager.PopPage();
        }

        public override void OnLoad(Dictionary<string, object> intent)
        {
            // What really pisses me off is I wrote this but have no idea why it works. I just kept throwing stuff 
            // in there until it worked. The selection systems for these menus absolutely sucks and make no sense.
            if (WasReload) 
            {
                PageLayout.Select(InterfaceElement.Directions.Top, false);
                _contentLayout.Deselect(false);
                _contentLayout.Select(1,true);
            }
            else
            {
                PageLayout.Deselect(false);
                PageLayout.Select(InterfaceElement.Directions.Top, false);
            }
            WasReload = false;
        }

        public void PressButtonDialogFontChange(bool newState)
        {
            WasReload = true;
            GameSettings.VarWidthFont = newState;
            Resources.SetGameFont();
            Game1.UiPageManager.Reload(_contentManager);
        }
    }
}