using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.GameObjects;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class ModifiersPage : InterfacePage
    {
        private readonly InterfaceListLayout _modifiersLayout;
        private readonly InterfaceListLayout _contentLayout;
        private readonly InterfaceListLayout _bottomBar;
        public static bool _reloadMenus;
        private bool _showTooltip;

        public ModifiersPage(int width, int height)
        {
            EnableTooltips = true;

            var buttonWidth = 320;
            var buttonHeight = 16;

            // Modifiers Settings Layout
            _modifiersLayout = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };
            _modifiersLayout.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_mods_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));
            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize) - 12), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Slider: Extra Enemy HP
            var enemyHPSlider = new InterfaceSlider(Resources.GameFont, "settings_mods_enemy_hp",
                buttonWidth, new Point(1, 2), 0, 30, 1, GameSettings.EnemyBonusHP,
                number => { GameSettings.EnemyBonusHP = number; })
                { SetString = number => EnemyHPSliderAdjustment(number) };
            _contentLayout.AddElement(enemyHPSlider);

            // Slider: Damage Taken Multiplier
            var damageTakenSlider = new InterfaceSlider(Resources.GameFont, "settings_mods_damage",
                buttonWidth, new Point(1, 2), 0, 10, 1, GameSettings.DmgMultiplier,
                number => { GameSettings.DmgMultiplier = number; })
                { SetString = number => DamageTakenSliderAdjustment(number) };
            _contentLayout.AddElement(damageTakenSlider);

            // Slider: Movement Speed
            var movementSlider = new InterfaceSlider(Resources.GameFont, "settings_mods_movespeed",
                buttonWidth, new Point(1, 2), 0, 10, 1, (int)(GameSettings.MoveSpeedAdded * 10),
                number => { GameSettings.MoveSpeedAdded = number / 10f; })
                { SetString = number => AddedMoveSpeedSliderAdjustment(number) };
            _contentLayout.AddElement(movementSlider);

            // Button: No Damage Launch
            var toggleNoDmgLaunch = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_mods_dmglaunch", GameSettings.NoDamageLaunch, 
                newState => { GameSettings.NoDamageLaunch = newState; });
            _contentLayout.AddElement(toggleNoDmgLaunch);

            // Button: No Heart Drops
            var toggleNoHearts = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_mods_nohearts", GameSettings.NoHeartDrops, 
                newState => { GameSettings.NoHeartDrops = newState; });
            _contentLayout.AddElement(toggleNoHearts);

            // Button: Sword Block Projectiles
            var toggleSwordBlock = InterfaceToggle.GetToggleButton(new Point(buttonWidth, buttonHeight), new Point(5, 2),
                "settings_mods_swordblock", GameSettings.SwordBlock, 
                newState => { GameSettings.SwordBlock = newState; });
            _contentLayout.AddElement(toggleSwordBlock);

            // Bottom Bar / Back Button:
            _bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));
            _modifiersLayout.AddElement(_contentLayout);
            _modifiersLayout.AddElement(_bottomBar);
            PageLayout = _modifiersLayout;
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

        private string EnemyHPSliderAdjustment(int number)
        {
            ObjLives.RestoreDefaultHP();
            ObjLives.AddToEnemyHP(number);
            return ": " + number;
        }

        private string DamageTakenSliderAdjustment(int number)
        {
            return ": " + number + "x";
        }

        private string AddedMoveSpeedSliderAdjustment(int number)
        {
            float addmove = (float)(number / 10f);
            int percent = number * 10;
            MapManager.ObjLink.AlterMoveSpeed(addmove);
            return ": " + percent + "%";
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
            if (_modifiersLayout.SelectionIndex == 2)
                return  Game1.LanguageManager.GetString("tooltip_default", "error");

            // Detect the chosen button by checking the content InterfaceListLayout.
            int index = _contentLayout.SelectionIndex;
            string tooltip = "Select an option to view its tooltip.";

            // Use the selected index to determine which tooltip to show.
            switch (index) 
            {
                case 0:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_enemy_hp", "error"); break; }
                case 1:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_damage", "error"); break; }
                case 2:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_movespeed", "error"); break; }
                case 3:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_dmglaunch", "error"); break; }
                case 4:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_nohearts", "error"); break; }
                case 5:  { tooltip = Game1.LanguageManager.GetString("tooltip_mods_swordblock", "error"); break; }
            }
            // Display the tooltip in the tooltip window.
            return tooltip;
        }
    }
}