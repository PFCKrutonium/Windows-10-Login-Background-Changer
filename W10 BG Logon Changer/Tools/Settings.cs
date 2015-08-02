﻿using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using W10_BG_Logon_Changer.Tools.Customs;

namespace W10_BG_Logon_Changer.Tools
{
    public static class Settings
    {
        private static SerializableDictionary<string, object> _settingsObject;

        public static void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Config.SettingsFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(Config.SettingsFilePath));

            using (var fileStream = new FileStream(Config.SettingsFilePath, FileMode.Create))
            {
                IFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, _settingsObject);
            }
        }

        public static void Load()
        {
            if (File.Exists(Config.SettingsFilePath))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(Config.SettingsFilePath, FileMode.Open))
                    {
                        IFormatter bf = new BinaryFormatter();
                        _settingsObject = (SerializableDictionary<string, object>)bf.Deserialize(fileStream);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine("[Settings Error] {0}", e.ToString());
                }
            }
            if (_settingsObject == null)
            {
                _settingsObject = new SerializableDictionary<string, object>();
            }
        }

        public static T Get<T>(string key, T @default)
        {
            if (!_settingsObject.ContainsKey(key)) return @default;
            if (_settingsObject[key] != null)
            {
                return (T)_settingsObject[key];
            }

            return @default;
        }

        public static void Set<T>(string key, T @value)
        {
            if (_settingsObject.ContainsKey(key) && _settingsObject[key] != null)
            {
                var test = _settingsObject[key].GetType();

                if (@value.GetType() == test)
                {
                    _settingsObject[key] = @value;
                }

                return;
            }

            _settingsObject.Add(key, @value);
        }
    }
}