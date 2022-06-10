﻿using System.Collections.Generic;

namespace saturn2
{
    public class SettingsFile
    {
        Dictionary<string, string> settings = new Dictionary<string, string>();
        public string this[string key]
        {
            get
            {
                try
                {
                    return settings[key];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                settings[key] = value;
            }
        }

        public SettingsFile(string text)
        {
            foreach (string line in text.Replace("\r", "").Split('\n'))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                string key = line.Substring(0, line.IndexOf(':'));
                string value = line.Remove(0, key.Length + 1).Trim();

                this[key] = value;
            }
        }

        public SettingsFile()
        {

        }

        public override string ToString()
        {
            string text = "# saturn settings";
            foreach (string key in settings.Keys)
            {
                text += "\r\n" + key + ": " + settings[key];
            }

            return text;
        }
    }
}
