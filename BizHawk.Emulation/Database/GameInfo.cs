﻿using System.Collections.Generic;
using System.Linq;

namespace BizHawk
{
    public enum RomStatus
    {
        GoodDump,
        BadDump,
        Homebrew,
        TranslatedRom,
        Hack,
        BIOS,
        Overdump,
        NotInDatabase
    }

    public class GameInfo
    {
        public string Name;
        public string System;
        public string Hash;
        public RomStatus Status;
        public bool NotInDatabase = true;

        private Dictionary<string, string> Options = new Dictionary<string, string>();

        public GameInfo() {}

        internal GameInfo(CompactGameInfo cgi)
        {
            Name = cgi.Name;
            System = cgi.System;
            Hash = cgi.Hash;
            Status = cgi.Status;
            NotInDatabase = false;
            ParseOptionsDictionary(cgi.MetaData);
        }

        public void AddOption(string option)
        {
            Options[option] = "";
        }

        public void AddOption(string option, string param)
        {
            Options[option] = param;
        }

        public void RemoveOption(string option)
        {
            Options.Remove(option);
        }

        public bool this[string option]
        {
            get { return Options.ContainsKey(option); }
        }

        public bool OptionPresent(string option)
        {
            return Options.ContainsKey(option);
        }

        public string OptionValue(string option)
        {
            if (Options.ContainsKey(option))
                return Options[option];
            return null;
        }

        public ICollection<string> GetOptions()
        {
            return Options.Keys;

        }
        public IDictionary<string,string> GetOptionsDict()
        {
            return new ReadOnlyDictionary<string,string>(Options);
        }

        private void ParseOptionsDictionary(string metaData)
        {
            if (string.IsNullOrEmpty(metaData))
                return;

            var options = metaData.Split(';').Where(opt => string.IsNullOrEmpty(opt) == false).ToArray();

            foreach (var opt in options)
            {
                var parts = opt.Split('=');
                var key = parts[0];
                var value = parts.Length > 1 ? parts[1] : "";
                Options[key] = value;
            }
        }
    }
}
