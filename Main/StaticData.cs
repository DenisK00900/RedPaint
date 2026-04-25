using System;
using System.Collections.Generic;
using System.IO; // <-- Добавлено для работы с путями
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class StaticData
    {
        Maincode parent;

        private const string SETTINGS_RELATIVE_PATH = "Data/Settings/Settings.txt";

        public void LoadSettings(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                bool reading = false;

                foreach (var rawLine in lines)
                {
                    var line = rawLine.Trim();

                    if (line.StartsWith("start;", StringComparison.OrdinalIgnoreCase))
                    {
                        reading = true;
                        continue;
                    }
                    if (line.StartsWith("end;", StringComparison.OrdinalIgnoreCase))
                    {
                        reading = false;
                        break;
                    }
                    if (!reading || string.IsNullOrWhiteSpace(line) || !line.Contains("="))
                        continue;

                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length != 2)
                        continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim().TrimEnd(';').Trim();

                    switch (key)
                    {
                        case "res":
                            res = ParseVector2(value);
                            break;
                        case "isFullScreen":
                            if (bool.TryParse(value, out bool fs))
                                isFullScreen = fs;
                            break;
                        case "version":
                            version = value;
                            break;
                        case "standartAPIkey":
                            standartKey = value;
                            break;
                        case "standartAPIkeyActuality":
                            standartKeyDate = value;
                            break;
                        case "promnt":
                            promnt = value;
                            break;
                        case "negativePromnt":
                            negativePromnt = value;
                            break;
                        case "isDevToolsOn":
                            if (bool.TryParse(value, out bool dev))
                                isDevToolsOn = dev;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[StaticData] Error loading settings: {ex.Message}");
            }
        }

        private Vector2 ParseVector2(string input)
        {
            var cleaned = input.Trim('(', ')', ' ');
            var parts = cleaned.Split(',');

            if (parts.Length == 2 &&
                float.TryParse(parts[0].Trim(), out float x) &&
                float.TryParse(parts[1].Trim(), out float y))
            {
                return new Vector2(x, y);
            }

            return new Vector2(1920, 1080);
        }

        public StaticData(Maincode mc)
        {
            parent = mc;

            res = new Vector2(1920, 1080);
            isFullScreen = false;
            currPalletre = 0;
            LoadedPalletres = new AppPalletre[1];
            LoadedPalletres[0] = new AppPalletre();
            version = "undef";
            isDevToolsOn = false;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.Combine(baseDir, SETTINGS_RELATIVE_PATH);

            absolutePath = absolutePath.Replace('\\', '/');

            LoadSettings(absolutePath);
        }

        public Vector2 res;
        public bool isFullScreen;
        public int currPalletre;
        public AppPalletre[] LoadedPalletres;
        public String version;
        public bool isDevToolsOn;

        public string standartKey;
        public string standartKeyDate;
        public bool useStandratKey = false;

        public string userKey = "";

        public string promnt;
        public string negativePromnt;

        public int AIsteps = 20;
        public float AIscale = 5.0f;
        public int AIseed = -1;
    }
}