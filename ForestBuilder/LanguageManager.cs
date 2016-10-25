//Copyright: ManagedCode
//2014. 10.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForestBuilder
{
    public class LanguageManager
    {
        private const string LANGUAGE_FILE = "language.txt";
        public class Language
        {
            public Dictionary<string, string> Words;
            public Language()
            {
                Words = new Dictionary<string, string>();
            }
        }
        private Dictionary<string, Language> languages;
        public Language CurrentLanguage
        {
            get
            {
                return languages[CurrentCulture];
            }
        }
        private string CurrentCulture;
        public LanguageManager()
        {
            languages = new Dictionary<string, Language>();
            string[] lines = System.IO.File.ReadAllLines(LANGUAGE_FILE);
            string currentSection = "";
            foreach (string line in lines)
            {
                string s = line.Trim();
                if (s.Contains(';'))
                    s = s.Substring(0, s.IndexOf(';'));
                if (s == "")
                    continue;
                if (s[0] == '[')
                {
                    if (!s.Contains(':') || !s.Contains(']'))
                        continue;
                    currentSection = s.Substring(s.IndexOf(':') + 1, s.IndexOf(']') - s.IndexOf(':') - 1);
                    languages.Add(currentSection, new Language());
                }
                if (s.Contains('='))
                {
                    string key = s.Substring(0, s.IndexOf('=') - 1).Trim();
                    string value = s.Substring(s.IndexOf('"') + 1, s.IndexOf('"', s.IndexOf('"') + 1) - s.IndexOf('"') - 1);
                    if (key == "CurrentCulture")
                    {
                        CurrentCulture = value;
                        continue;
                    }
                    languages[currentSection].Words.Add(key, value);
                }
            }
            if (CurrentCulture == null)
            {
                CurrentCulture = System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            }
            {
                bool found = false;
                foreach(string key in languages.Keys)
                {
                    if (key == CurrentCulture)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    CurrentCulture = "eng";
                }
            }
        }
    }
}
