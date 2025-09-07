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
    class ReduxOptionsPage : InterfacePage
    {
        private readonly ContentManager _contentManager;
        private readonly InterfaceListLayout _contentLayout;
        public static bool _reloadMenus;

        public ReduxOptionsPage(int width, int height, ContentManager content)
        {
            _contentManager = content;
            var buttonWidth = 320;

            // Redux Settings Layout:
            var reduxOptionsList = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            reduxOptionsList.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_redux_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Original Menu Border:
            var toggleMenuBricks = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_redux_menubricks", GameSettings.OldMenuBorder, newState => { PressButtonToggleMenuBricks(newState); });
            _contentLayout.AddElement(toggleMenuBricks);

            // Variable Width Font Toggle:
            var toggleVariableFont = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_redux_vwfont", GameSettings.VarWidthFont, newState => { PressButtonDialogFontChange(newState); });
            _contentLayout.AddElement(toggleVariableFont);

            // Disable Helper Interactions:
            var toggleHelperText = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_redux_nohelptext", GameSettings.NoHelperText, newState => { PressButtonToggleHelpers(newState); });
            _contentLayout.AddElement(toggleHelperText);

            // Disable Censorship:
            var toggleUncensored = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_redux_uncensor", GameSettings.Uncensored, newState => { PressButtonToggleUncensored(newState); });
            _contentLayout.AddElement(toggleUncensored);

            // Enable No Missables:
            var toggleUnmissables = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 18), new Point(5, 2),
                "settings_redux_unmissables", GameSettings.Unmissables, newState => { PressButtonToggleUnmissables(newState); });
            _contentLayout.AddElement(toggleUnmissables);

            // Bottom Bar / Back Button:
            var bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));

            // Add everything to interface layout.
            reduxOptionsList.AddElement(_contentLayout);
            reduxOptionsList.AddElement(bottomBar);
            PageLayout = reduxOptionsList;
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
            if (_reloadMenus) 
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
            _reloadMenus = false;
        }

        public void PressButtonToggleMenuBricks(bool newState)
        {
            GameSettings.OldMenuBorder = newState;
            var texture = newState
                ? _contentManager.Load<Texture2D>("Menu/menuBackgroundAlt")
                : _contentManager.Load<Texture2D>("Menu/menuBackground");

            var menuScreen = (MenuScreen)Game1.ScreenManager.GetScreen(Values.ScreenNameMenu);
            menuScreen?.SetBackground(texture);
        }

        public void PressButtonDialogFontChange(bool newState)
        {
            _reloadMenus = true;
            GameSettings.VarWidthFont = newState;
            Game1.GameManager.InGameOverlay.TextboxOverlay.ResolutionChange();
            Game1.UiPageManager.Reload(_contentManager);
        }

        public void PressButtonToggleHelpers(bool newState)
        {
            // Set the new state and refresh the items group.
            GameSettings.NoHelperText = newState;
            Game1.GameManager.ItemManager.Load();
        }

        public void PressButtonToggleUncensored(bool newState)
        {
            // Set the new state and refresh the fonts and items group.
            GameSettings.Uncensored = newState;
            Game1.GameManager.InGameOverlay.TextboxOverlay.ResolutionChange();
            Game1.GameManager.ItemManager.Load();
        }

        public void PressButtonToggleUnmissables(bool newState) 
        {
            GameSettings.Unmissables = newState;
        }
    }
}