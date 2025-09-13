using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Screens;

namespace ProjectZ.InGame.Things
{
    internal class Resources
    {
        public class Texture
        {
            public string Name;
            public Texture2D SprTexture;

            public Texture(string name)
            {
                Name = name;
            }
        }
        public static Effect RoundedCornerEffect;
        public static Effect BlurEffect;
        public static Effect RoundedCornerBlurEffect;
        public static Effect BlurEffectV;
        public static Effect BlurEffectH;
        public static Effect BBlurEffectV;
        public static Effect BBlurEffectH;
        public static Effect BBlurMapping;
        public static Effect FullShadowEffect;
        public static Effect SaturationEffect;
        public static Effect WobbleEffect;
        public static Effect CircleShader;
        public static Effect LightShader;
        public static Effect LightFadeShader;
        public static Effect ThanosShader;

        // some sprites need different parameters set
        // we try to use as little different sprites effects as possible
        public static SpriteShader DamageSpriteShader0;
        public static SpriteShader DamageSpriteShader1;
        public static SpriteShader CloudShader;
        public static SpriteShader ThanosSpriteShader0;
        public static SpriteShader ThanosSpriteShader1;
        public static SpriteShader WindFishShader;
        public static SpriteShader ColorShader;
        public static SpriteShader ShockShader0;
        public static SpriteShader ShockShader1;

        public static SpriteFont EditorFont, EditorFontMonoSpace, EditorFontSmallMonoSpace;
        public static SpriteFont GameHeaderFont;
        public static SpriteFont FontCredits, FontCreditsHeader;
        public static SpriteFont smallFont, smallFont_redux, smallFont_vwf, smallFont_vwf_redux;

        public static SpriteFont GameFont => (GameSettings.VarWidthFont, GameSettings.Uncensored) 
            switch
            {
                (true,  true)  => smallFont_vwf_redux,
                (true,  false) => smallFont_vwf,
                (false, true)  => smallFont_redux,
                (false, false) => smallFont
            };

        public static Texture2D EditorEyeOpen, EditorEyeClosed, EditorIconDelete;
        public static Texture2D SprWhite, SprTiledBlock, SprObjectsAnimated, SprNpCs, SprNpCsRedux;
        public static Texture2D SprEnemies, SprMidBoss, SprNightmares;
        public static Texture2D SprShadow;
        public static Texture2D SprBlurTileset;
        public static Texture2D SprLink, SprLinkCloak;
        public static Texture2D SprGameSequences;
        public static Texture2D SprGameSequencesFinal;
        public static Texture2D SprFog;
        public static Texture2D SprLight;
        public static Texture2D SprLightRoomH;
        public static Texture2D SprLightRoomV;
        public static Texture2D NoiseTexture;
        public static Texture2D SprIconOptions, SprIconErase, SprIconCopy, EditorIconEdit, EditorIconSelect;
        public static Texture2D SprPhotosEng, SprPhotosEngRedux, SprPhotosEsp, SprPhotosEspRedux;

        public static Texture2D SprObjectsEng, SprObjectsEsp;
        public static Texture2D SprObjects => (Game1.LanguageManager.CurrentLanguageIndex) 
            switch
            {
                1 => SprObjectsEng,
                _ => SprObjectsEng
            };

        public static Texture2D SprMiniMapEng, SprMiniMapEsp;
        public static Texture2D SprMiniMap => (Game1.LanguageManager.CurrentLanguageIndex) 
            switch
            {
                1 => SprMiniMapEsp,
                _ => SprMiniMapEng
            };

        public static Texture2D SprItemEng, SprItemEngRedux, SprItemEsp, SprItemEspRedux;
        public static Texture2D SprItem => (Game1.LanguageManager.CurrentLanguageIndex, GameSettings.Uncensored) 
            switch
            {
                (1, false) => SprItemEsp,
                (_, false) => SprItemEng,
                (1, true)  => SprItemEspRedux,
                (_, true)  => SprItemEngRedux
            };

        public static List<Texture> TextureList = new();
        public static List<Texture> ReloadQueue = new();

        // TODO: Sprite atlas dictionaries should probably be converted into an array eventually so it's easier to manage when more languages
        // graphics are implemented. This should both make it easier to track and clean up the code since language is an index. But with that
        // said, when more than two languages are implemented, it's going to be a pain in the ass to code them all either way.

        public static Dictionary<string, DictAtlasEntry> SpriteAtlas = new();
        public static Dictionary<string, DictAtlasEntry> SpriteAtlasRedux = new();

        public static Dictionary<string, DictAtlasEntry> SpriteAtlasEsp = new();
        public static Dictionary<string, DictAtlasEntry> SpriteAtlasEspRedux = new();

        public static Dictionary<string, int> TilesetSizes = new();
        public static Dictionary<string, SoundEffect> SoundEffects = new();

        public static int GameFontHeight = 10;
        public static int EditorFontHeight;

        // resources needed to start showing the intro
        public static void LoadIntro(GraphicsDevice graphics, ContentManager content)
        {
            // TODO: make sure to only load the stuff needed; BlurEffect is not needed but needs changes to not load it here

            SprWhite = new Texture2D(graphics, 1, 1);
            SprWhite.SetData(new[] { Color.White });

            LoadTexturesFromFolder(Values.PathContentFolder + "/Intro/");

            BlurEffect = content.Load<Effect>("Shader/EffectBlur");
            RoundedCornerBlurEffect = content.Load<Effect>("Shader/RoundedCornerEffectBlur");

            AddSoundEffect(content, "D378-15-0F");
            AddSoundEffect(content, "D378-12-0C");
            AddSoundEffect(content, "D378-25-19");
        }
        
        public static void LoadTextures(GraphicsDevice graphics, ContentManager content)
        {
            // load the tileset sizes
            LoadTilesetSizes();

            LoadTexture(out SprGameSequences, Values.PathContentFolder + "Sequences/game sequences.png");
            LoadTexture(out SprGameSequencesFinal, Values.PathContentFolder + "Sequences/end sequence.png");
            LoadTexture(out SprPhotosEng, Values.PathContentFolder + "Photo Mode/photos.png");
            LoadTexture(out SprPhotosEngRedux, Values.PathContentFolder + "Photo Mode/photos_redux.png");
            LoadTexture(out SprPhotosEsp, Values.PathContentFolder + "Photo Mode/photos_esp.png");
            LoadTexture(out SprPhotosEspRedux, Values.PathContentFolder + "Photo Mode/photos_redux_esp.png");
            LoadTexture(out _, Values.PathContentFolder + "Editor/editorIcons4x.png");
            LoadTexture(out _, Values.PathContentFolder + "ui.png");
            LoadTexture(out _, Values.PathContentFolder + "ui_esp.png");

            LoadTexturesFromFolder(Values.PathContentFolder + "/Sequences/");
            LoadTexturesFromFolder(Values.PathContentFolder + "/Light/");

            // load all the tileset textures
            LoadTexturesFromFolder(Values.PathTilesetFolder);
            LoadTexturesFromFolder(Values.PathMapObjectFolder);

            // Static resources that never change.
            SprLink = GetTexture("link0.png");
            SprLinkCloak = GetTexture("link_cloak.png");
            SprEnemies = GetTexture("enemies.png");
            SprObjectsAnimated = GetTexture("objects animated.png");
            SprMidBoss = GetTexture("midboss.png");
            SprNightmares = GetTexture("nightmares.png");
            SprBlurTileset = GetTexture("blur tileset.png");

            // Dynamic resources that change depending on options set.
            SprObjectsEng = GetTexture("objects.png");
            SprObjectsEsp = GetTexture("objects_esp.png");
            SprMiniMapEng = GetTexture("minimap.png");
            SprMiniMapEsp = GetTexture("minimap_esp.png");
            SprNpCs = GetTexture("npcs.png");
            SprNpCsRedux = GetTexture("npcs_redux.png");
            SprItemEng = GetTexture("items.png");
            SprItemEngRedux = GetTexture("items_redux.png");
            SprItemEsp = GetTexture("items_esp.png");
            SprItemEspRedux = GetTexture("items_redux_esp.png");

            // load fonts
            EditorFont = content.Load<SpriteFont>("Fonts/editor font");
            EditorFontHeight = (int)EditorFont.MeasureString("H").Y;
            EditorFontMonoSpace = content.Load<SpriteFont>("Fonts/editor mono font");
            EditorFontSmallMonoSpace = content.Load<SpriteFont>("Fonts/editor small mono font");
            GameHeaderFont = content.Load<SpriteFont>("Fonts/newHeaderFont");
            FontCredits = content.Load<SpriteFont>("Fonts/credits font");
            FontCreditsHeader = content.Load<SpriteFont>("Fonts/credits header font");
            smallFont = content.Load<SpriteFont>("Fonts/smallFont");
            smallFont_redux = content.Load<SpriteFont>("Fonts/smallFont_redux");
            smallFont_vwf = content.Load<SpriteFont>("Fonts/smallFont_vwf");
            smallFont_vwf_redux = content.Load<SpriteFont>("Fonts/smallFont_vwf_redux");

            // load textures
            SprTiledBlock = new Texture2D(graphics, 2, 2);
            SprTiledBlock.SetData(new[] { Color.White, Color.LightGray, Color.LightGray, Color.White });

            EditorEyeOpen = content.Load<Texture2D>("Editor/eye_open");
            EditorEyeClosed = content.Load<Texture2D>("Editor/eye_closed");
            EditorIconDelete = content.Load<Texture2D>("Editor/delete");
            EditorIconEdit = content.Load<Texture2D>("Editor/edit");
            EditorIconSelect = content.Load<Texture2D>("Editor/select");

            SprLight = content.Load<Texture2D>("Light/light");
            SprLightRoomH = content.Load<Texture2D>("Light/ligth room");
            SprLightRoomV = content.Load<Texture2D>("Light/ligth room vertical");

            SprShadow = content.Load<Texture2D>("Light/shadow");
            LoadContentTextureWithAtlas(content, "Light/doorLight");

            SprIconOptions = content.Load<Texture2D>("Menu/gearIcon");
            SprIconErase = content.Load<Texture2D>("Menu/trashIcon");
            SprIconCopy = content.Load<Texture2D>("Menu/copyIcon");

            // need to have pre multiplied alpha
            SprFog = content.Load<Texture2D>("Objects/fog");

            // load shader
            RoundedCornerEffect = content.Load<Effect>("Shader/RoundedCorner");
            BlurEffectH = content.Load<Effect>("Shader/BlurH");
            BlurEffectV = content.Load<Effect>("Shader/BlurV");
            BBlurEffectH = content.Load<Effect>("Shader/BBlurH");
            BBlurEffectV = content.Load<Effect>("Shader/BBlurV");
            FullShadowEffect = content.Load<Effect>("Shader/FullShadowEffect");
            // used in the inventory
            SaturationEffect = content.Load<Effect>("Shader/SaturationFilter");
            WobbleEffect = content.Load<Effect>("Shader/WobbleShader");
            CircleShader = content.Load<Effect>("Shader/CircleShader");
            LightShader = content.Load<Effect>("Shader/LightShader");
            LightFadeShader = content.Load<Effect>("Shader/LightFadeShader");

            var cloudShader = content.Load<Effect>("Shader/ColorCloud");
            CloudShader = new SpriteShader(cloudShader);
            CloudShader.FloatParameter.Add("scaleX", 1);
            CloudShader.FloatParameter.Add("scaleY", 1);

            NoiseTexture = GetTexture("thanos noise.png");
            ThanosShader = content.Load<Effect>("Shader/ThanosShader");
            ThanosShader.Parameters["NoiceTexture"].SetValue(NoiseTexture);
            // only works for sprites using the sequence sprite
            ThanosShader.Parameters["Scale"].SetValue(new Vector2(
                    (float)SprGameSequencesFinal.Width / NoiseTexture.Width,
                    (float)SprGameSequencesFinal.Height / NoiseTexture.Height));

            ThanosSpriteShader0 = new SpriteShader(ThanosShader);
            ThanosSpriteShader0.FloatParameter.Add("Percentage", 0);
            ThanosSpriteShader1 = new SpriteShader(ThanosShader);
            ThanosSpriteShader1.FloatParameter.Add("Percentage", 0);

            WindFishShader = new SpriteShader(content.Load<Effect>("Shader/WaleShader"));
            WindFishShader.FloatParameter.Add("Offset", 0);
            WindFishShader.FloatParameter.Add("Period", 0);

            ColorShader = new SpriteShader(content.Load<Effect>("Shader/ColorShader"));

            var damageShader = content.Load<Effect>("Shader/DamageShader");

            // crow needs mark1 to have a value bigger than 0.605333
            DamageSpriteShader0 = new SpriteShader(damageShader);
            DamageSpriteShader0.FloatParameter.Add("mark0", 0.1f);
            DamageSpriteShader0.FloatParameter.Add("mark1", 0.725f);

            // stone hinox needs mark1 to be below 0.553
            DamageSpriteShader1 = new SpriteShader(damageShader);
            DamageSpriteShader1.FloatParameter.Add("mark0", 0.1f);
            DamageSpriteShader1.FloatParameter.Add("mark1", 0.55f);

            var shockShader = content.Load<Effect>("Shader/ShockEffect");

            ShockShader0 = new SpriteShader(shockShader);
            ShockShader0.FloatParameter.Add("mark0", 0.0f);
            ShockShader0.FloatParameter.Add("mark1", 0.2675f);
            ShockShader0.FloatParameter.Add("mark2", 0.725f);

            ShockShader1 = new SpriteShader(shockShader);
            ShockShader1.FloatParameter.Add("mark0", 0.0f);
            ShockShader1.FloatParameter.Add("mark1", 0.35f);
            ShockShader1.FloatParameter.Add("mark2", 0.625f);
        }

        public static void LoadSounds(ContentManager content)
        {
            // load all the sound effects
            var soundEffectFiles = Directory.GetFiles(content.RootDirectory + "/SoundEffects").ToList();
            foreach (var path in soundEffectFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                AddSoundEffect(content, fileName);
            }
        }

        public static void AddSoundEffect(ContentManager content, string fileName)
        {
            var soundEffect = content.Load<SoundEffect>("SoundEffects/" + fileName);

            // try add is used because some files may already be loaded for the intro sequence
            SoundEffects.TryAdd(fileName, soundEffect);
        }

        public static string FindAtlasFile(string textureName)
        {
            var parts = textureName
                .Replace(".png", "")
                .Split('_')
                .Where(name => name != "redux" && name != "esp");

            return string.Join("_", parts) + ".atlas";
        }

        public static void LoadContentTextureWithAtlas(ContentManager content, string filePath)
        {
            var texture = content.Load<Texture2D>(filePath);
            var atlasPath = FindAtlasFile(Values.PathContentFolder + filePath);
            SpriteAtlasSerialization.LoadSourceDictionary(texture, atlasPath, SpriteAtlas);
        }

        public static void LoadTexturesFromFolder(string path)
        {
            var texturePaths = Directory.GetFiles(path).ToList();

            foreach (var filePath in texturePaths)
            {
                var _filePath = filePath.Replace("//","/");

                if (!_filePath.Contains(".png"))
                    continue;

                var newTexture = new Texture(Path.GetFileName(_filePath));

                LoadTexture(out newTexture.SprTexture, _filePath);
                TextureList.Add(newTexture);
            }
        }

        public static void LoadTexture(out Texture2D texture, string strFilePath)
        {
            // Sprite sheets use an "atlas" file that contains various data about each sprite like position, rectangle size, etc. When loading
            // a texture, load its corresponding atlas file. If it's a "_redux" variation, we can use the same atlas file as the original, but
            // we store the actual textures in a separate "SpriteAtlas" to make it much easier to switch to those variations later.

            using Stream stream = File.Open(strFilePath, FileMode.Open);
            texture = Texture2D.FromStream(Game1.Graphics.GraphicsDevice, stream);

            string atlasFileName = FindAtlasFile(strFilePath);
            string[] strFileSplit = strFilePath.Split(new[] { '_', '.' });

            bool isSpanish = strFileSplit.Contains("esp");
            bool isRedux   = strFileSplit.Contains("redux");

            Dictionary<string, DictAtlasEntry> atlasDesignation = (isSpanish, isRedux) switch
            {
                (true,  false) => SpriteAtlasEsp,
                (false, false) => SpriteAtlas,
                (true,  true)  => SpriteAtlasEspRedux,
                (false, true)  => SpriteAtlasRedux,
            };
            SpriteAtlasSerialization.LoadSourceDictionary(texture, atlasFileName, atlasDesignation);
        }

        public static Texture2D GetTexture(string name)
        {
            for (var i = 0; i < TextureList.Count; i++)
            {
                if (TextureList[i].Name == name)
                    return TextureList[i].SprTexture;
            }
            return null;
        }

        public static Rectangle SourceRectangle(string id)
        {
            // We don't need to get any special atlas since all versions share the same dimensions.
            return SpriteAtlas.ContainsKey(id) ? SpriteAtlas[id].ScaledRectangle : Rectangle.Empty;
        }

        private static DictAtlasEntry GetSpriteInternal(string id, bool variation)
        {
            // All sprites use the current langage to search for variations, most sprites will also use "GameSettings.Uncensored" to determine if
            // there is a "redux" uncensored version, and the photograph sprites will use "GameSettings.PhotosColor" to get the colored versions.
            // All "search" chains in the switch below should end with "SpriteAtlas" as it will always contains an entry.

            var atlases = (Game1.LanguageManager.CurrentLanguageIndex, variation) switch
            {
                (1, false) => new[] { SpriteAtlasEsp, SpriteAtlas },
                (_, false) => new[] { SpriteAtlas },
                (1, true)  => new[] { SpriteAtlasEspRedux, SpriteAtlasEsp, SpriteAtlas },
                (_, true)  => new[] { SpriteAtlasRedux, SpriteAtlas }
            };
            // Check each atlas and see if it contains the ID of the sprite we are trying to load. If not, go to the next in the chain.
            foreach (var atlas in atlases)
                if (atlas.TryGetValue(id, out var sprite))
                    return sprite;

            // We should NEVER get here, but just as a fail safe check the standard "SpriteAtlas" one more time.
            return SpriteAtlas.TryGetValue(id, out var fallback) ? fallback : null;
        }

        public static DictAtlasEntry GetSprite(string id) =>
            GetSpriteInternal(id, GameSettings.Uncensored);

        public static DictAtlasEntry GetPhotoSprite(string id) =>
            GetSpriteInternal(id, GameSettings.PhotosColor);

        public static void LoadTilesetSizes()
        {
            var fileName = Path.Combine(Values.PathTilesetFolder, "tileset size.txt");

            if (!File.Exists(fileName))
                return;

            foreach (var line in File.ReadLines(fileName))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                var split = line.Split(':', 2, StringSplitOptions.TrimEntries);
                if (split.Length == 2 && int.TryParse(split[1], out var value))
                    TilesetSizes[split[0]] = value;
            }
        }

        public static void RefreshMenuBorderTexture(ContentManager content, int index)
        {
            var texture = index switch
            {
                0 => content.Load<Texture2D>("Menu/menuBackground"),
                1 => content.Load<Texture2D>("Menu/menuBackgroundB"),
                2 => content.Load<Texture2D>("Menu/menuBackgroundC")
            };
            var menuScreen = (MenuScreen)Game1.ScreenManager.GetScreen(Values.ScreenNameMenu);
            menuScreen?.SetBackground(texture);
        }

        public static void RefreshDynamicResources()
        {
            // Reload photo album photos so proper photos are displayed.
            Game1.GameManager.InGameOverlay.RefreshPhotoOverlay();

            // Refresh title screen resources so proper logo is displayed.
            Game1.ScreenManager.Intro.RefreshIntroResources();

            // Reload the UI textures (hearts, rupee icon, small key icon, game over, etc). 
            ItemDrawHelper.RefreshImagesUI();
        }
    }
}