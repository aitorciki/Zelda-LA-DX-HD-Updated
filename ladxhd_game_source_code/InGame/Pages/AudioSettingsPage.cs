using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Interface;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Pages
{
    class AudioSettingsPage : InterfacePage
    {
        private readonly InterfaceListLayout _audioSettingsLayout;
        private readonly InterfaceListLayout _contentLayout;
        private readonly InterfaceListLayout _bottomBar;
        private bool _showTooltip;

        public AudioSettingsPage(int width, int height)
        {
            EnableTooltips = true;

            // Audio Settings Layout
            _audioSettingsLayout = new InterfaceListLayout { Size = new Point(width, height - 12), Selectable = true };

            var buttonWidth = 320;

            _audioSettingsLayout.AddElement(new InterfaceLabel(Resources.GameHeaderFont, "settings_audio_header",
                new Point(buttonWidth, (int)(height * Values.MenuHeaderSize)), new Point(0, 0)));

            _contentLayout = new InterfaceListLayout { Size = new Point(width, (int)(height * Values.MenuContentSize - 12)), Selectable = true, ContentAlignment = InterfaceElement.Gravities.Top };

            // Slider: Music Volume
            _contentLayout.AddElement(new InterfaceSlider(Resources.GameFont, "settings_audio_music_volume",
                buttonWidth, new Point(1, 2), 0, 100, 5, GameSettings.MusicVolume, number => { GameSettings.MusicVolume = number; })
            { SetString = number => " " + number + "%" });

            // Slider: Sound Effects Volume
            _contentLayout.AddElement(new InterfaceSlider(Resources.GameFont, "settings_audio_effect_volume",
                buttonWidth, new Point(1, 2), 0, 100, 5, GameSettings.EffectVolume, number => 
                { 
                    Game1.GameManager.UpdateSoundEffects();
                    GameSettings.EffectVolume = number; 
                })
            { SetString = number => " " + number + "%" });

            // Button: Classic Music Tile Cues
            var toggleAudioClassic = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 16), new Point(5, 2),
                "settings_audio_classic_music", GameSettings.ClassicMusic, newState => { GameSettings.ClassicMusic = newState; });
            _contentLayout.AddElement(toggleAudioClassic);

            // Button: Mute Inactive Window
            var toggleAudioMute = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 16), new Point(5, 2),
                "settings_audio_mute_inactive", GameSettings.MuteInactive, newState => { GameSettings.MuteInactive = newState; });
            _contentLayout.AddElement(toggleAudioMute);

            // Button: Low Hearts Alarm
            var toggleHeartBeep = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 16), new Point(5, 2),
                "settings_audio_heartbeep", GameSettings.HeartBeep, newState => { GameSettings.HeartBeep = newState; });
            _contentLayout.AddElement(toggleHeartBeep);

            // Button: Mute Powerup Music
            var toggleNoPowerupMusic = InterfaceToggle.GetToggleButton(new Point(buttonWidth, 16), new Point(5, 2),
                "settings_audio_mute_powerups", GameSettings.MutePowerups, newState => { GameSettings.MutePowerups = newState; });
            _contentLayout.AddElement(toggleNoPowerupMusic);

            // Bottom Bar / Back Button:
            _bottomBar = new InterfaceListLayout() { Size = new Point(width, (int)(height * Values.MenuFooterSize)), Selectable = true, HorizontalMode = true };
            _bottomBar.AddElement(new InterfaceButton(new Point(100, 18), new Point(2, 4), "settings_menu_back", element => { Game1.UiPageManager.PopPage(); }));
            _audioSettingsLayout.AddElement(_contentLayout);
            _audioSettingsLayout.AddElement(_bottomBar);
            PageLayout = _audioSettingsLayout;
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
            if (_audioSettingsLayout.SelectionIndex == 2)
                return  Game1.LanguageManager.GetString("tooltip_default", "error");

            // Detect the chosen button by checking the content InterfaceListLayout.
            int index = _contentLayout.SelectionIndex;
            string tooltip = "Select an option to view its tooltip.";

            // Use the selected index to determine which tooltip to show.
            switch (index) 
            {
                case 0:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_music_volume", "error"); break; }
                case 1:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_effect_volume", "error"); break; }
                case 2:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_classic_music", "error"); break; }
                case 3:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_mute_inactive", "error"); break; }
                case 4:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_heartbeep", "error"); break; }
                case 5:  { tooltip = Game1.LanguageManager.GetString("tooltip_audio_mute_powerups", "error"); break; }
            }
            // Display the tooltip in the tooltip window.
            return tooltip;
        }
    }
}
