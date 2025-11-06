using System;
using System.IO;
using System.Globalization;
using System.Reflection;
using ProjectZ.InGame.GameObjects.Base;

namespace ProjectZ.InGame.Things
{
    public static class ModFile
    {
        public static void Parse(string modFile, dynamic inputClass)
        {
            foreach (string line in File.ReadAllLines(modFile))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                string[] splitLine = line.Split(new char[]{ '=', '/' });
                if (splitLine.Length < 2)
                    continue;

                string varName = splitLine[0].Trim();
                string varValue = splitLine[1].Trim();

                System.Diagnostics.Debug.WriteLine(varName);
                System.Diagnostics.Debug.WriteLine(varValue);

                FieldInfo field = inputClass.GetType().GetField(varName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                object convertedValue = Convert.ChangeType(varValue, field.FieldType, CultureInfo.InvariantCulture);
                field.SetValue(inputClass, convertedValue);
            }
        }
    }
}
