using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectZ.InGame.Things
{
    public class Language
    {
        public Dictionary<string, string> Strings => _languageStrings[CurrentLanguageIndex];

        private Dictionary<string, string>[] _languageStrings;

        public int CurrentLanguageIndex;
        public int CurrentSubLanguageIndex;

        public void Load()
        {
            // go through the .lng files and fill the _languageStrings dictionary array
            var files = Directory.GetFiles(Values.PathLanguageFolder);
            var languageStrings = new Dictionary<string, Dictionary<string, string>>();

            // the default (first) entry is english
            languageStrings.Add("eng", new Dictionary<string, string>());

            for (var i = 0; i < files.Length; i++)
            {
                var extension = Path.GetExtension(files[i]);
                if (extension == ".lng")
                {
                    var fileName = Path.GetFileNameWithoutExtension(files[i]);
                    var split = fileName.Split('_');
                    var lngName = "";

                    // eng.lng
                    if (split.Length == 1)
                        lngName = split[0];
                    // dialog_eng.lng
                    if (split.Length == 2)
                        lngName = split[1];

                    languageStrings.TryGetValue(lngName, out Dictionary<string, string> dict);

                    if (dict == null)
                    {
                        dict = new Dictionary<string, string>();
                        languageStrings.Add(lngName, dict);
                    }

                    if (split.Length == 1 || (split.Length == 2 && split[0] == "dialog"))
                        LoadFile(dict, files[i]);
                }
            }
            _languageStrings = languageStrings.Values.ToArray();
            CurrentLanguageIndex = Math.Clamp(CurrentLanguageIndex, 0, _languageStrings.Length - 1);
        }

        public string ReplacePlaceholderTag(string inputString)
        {
            // put in the players name
            inputString = inputString.Replace("[NAME]", Game1.GameManager.SaveName);
            inputString = inputString.Replace("[TRADE0]", "¯");
            inputString = inputString.Replace("[TRADE1]", "¢");
            inputString = inputString.Replace("[TRADE2]", "£");
            inputString = inputString.Replace("[TRADE3]", "¤");
            inputString = inputString.Replace("[TRADE4]", "¥");
            inputString = inputString.Replace("[TRADE5]", "¦");
            inputString = inputString.Replace("[TRADE6]", "§");
            inputString = inputString.Replace("[TRADE7]", "¨");
            inputString = inputString.Replace("[TRADE8]", "©");
            inputString = inputString.Replace("[TRADE9]", "ª");
            inputString = inputString.Replace("[TRADE10]", "«");
            inputString = inputString.Replace("[TRADE11]", "¬");
            inputString = inputString.Replace("[TRADE12]", "­");
            inputString = inputString.Replace("[TRADE13]", "®");
            inputString = inputString.Replace("[LEFT]", "°");
            inputString = inputString.Replace("[RIGHT]", "±");
            inputString = inputString.Replace("[DOWN]", "²");
            inputString = inputString.Replace("[UP]", "³");
            inputString = inputString.Replace("[DPAD]", "´");
            inputString = inputString.Replace("[SKULL]", "µ");
            inputString = inputString.Replace("[MARIN]", "¶");
            inputString = inputString.Replace("[LINK]", "·");
            return inputString;
        }

        public void LoadFile(Dictionary<string, string> dictionary, string fileName)
        {
            var reader = new StreamReader(fileName);

            while (!reader.EndOfStream)
            {
                var strLine = reader.ReadLine();
                var spacePosition = strLine.IndexOf(' ');

                if (spacePosition < 0 || strLine.StartsWith("//"))
                    continue;

                var strKey = strLine.Substring(0, spacePosition);

                // empty string
                if (spacePosition + 1 >= strLine.Length)
                {
                    dictionary.Add(strKey, "");
                    continue;
                }
                var strValue = strLine.Substring(spacePosition + 1);

                dictionary.Add(strKey, strValue);
            }
            reader.Close();
        }

        public string GetString(string strKey, string defaultString)
        {
            if (strKey == null)
                return "null";

            if (Strings.ContainsKey(strKey))
                defaultString = Strings[strKey];

            // use the english text if there is no translation
            else if (_languageStrings[0].ContainsKey(strKey))
                defaultString = _languageStrings[0][strKey];

            return defaultString;
        }

        public void ToggleLanguage()
        {
            CurrentLanguageIndex = (CurrentLanguageIndex + 1) % _languageStrings.Length;
        }
    }
}
