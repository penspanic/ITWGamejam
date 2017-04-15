using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Xml;

namespace ITW.Config
{
    public static class ConfigManager
    {
        public interface IParsable<T>
        {
            void FromString(string s);
        }

        /// <summary>
        /// Wrapper class of Win32API WritePrivateProfileString, GetPrivateProfileString.
        /// Must call Initialize() before use.
        /// </summary>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder sb, int length, string filePath);

        private static Dictionary<string, Dictionary<string, string>> sections;
        private static string filePath;

        public const int MAX_LENGTH = 255;

        public static bool Initialize(string filePath)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                return false;
            }

            ConfigManager.filePath = Environment.CurrentDirectory + "\\" + filePath;
            return true;
        }

        public static bool Exists<T>(string section, string key)
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, null, sb, MAX_LENGTH, filePath);
            return sb.ToString() == null;
        }

        public static int GetInt(string section, string key, int defaultValue = 0)
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, defaultValue.ToString(), sb, MAX_LENGTH, filePath);
            return int.Parse(sb.ToString());
        }

        public static float GetFloat(string section, string key, float defaultValue = 0f)
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, defaultValue.ToString(), sb, MAX_LENGTH, filePath);
            return float.Parse(sb.ToString());
        }

        public static string GetString(string section, string key, string defaultValue = "")
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, defaultValue.ToString(), sb, MAX_LENGTH, filePath);
            return sb.ToString();
        }

        /// <summary>
        /// Get value of T type.
        /// T must implement interface IParsable, and must have 0 argument constructor.
        /// </summary>
        public static T Get<T>(string section, string key, T defaultValue) where T : IParsable<T>, new()
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, defaultValue.ToString(), sb, MAX_LENGTH, filePath);
            T t = new T();
            t.FromString(sb.ToString());
            return t;
        }

        public static void SetValue<T>(string section, string key, T value)
        {
            WritePrivateProfileString(section, key, value.ToString(), filePath);
        }
    }
}