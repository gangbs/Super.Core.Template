using Microsoft.Win32;
using System;
using System.Linq;

namespace Super.Core.Infrastruct.Windows
{
    public class RegistryOperation
    {
        readonly RegistryKey _rootKey;

        public RegistryOperation()
        {
            _rootKey = Registry.LocalMachine;
        }

        public RegistryOperation(RegistryHive hKey)
        {
            switch (hKey)
            {
                case RegistryHive.ClassesRoot:
                    _rootKey = Registry.ClassesRoot;
                    break;
                case RegistryHive.CurrentConfig:
                    _rootKey = Registry.CurrentConfig;
                    break;
                case RegistryHive.CurrentUser:
                    _rootKey = Registry.CurrentUser;
                    break;
                case RegistryHive.LocalMachine:
                    _rootKey = Registry.LocalMachine;
                    break;
                case RegistryHive.Users:
                    _rootKey = Registry.Users;
                    break;
                default:
                    _rootKey = Registry.LocalMachine;
                    break;
            }
        }

        public RegistryOperation(RegistryHive hKey, string machineName)
        {
            _rootKey = RegistryKey.OpenRemoteBaseKey(hKey, machineName);
        }

        public RegistryKey AddKey(string path)
        {
            RegistryKey key = _rootKey.CreateSubKey(path);
            return key;
        }

        public RegistryKey OpenKey(string path, bool writable = false)
        {
            RegistryKey key = _rootKey.OpenSubKey(path, writable);
            return key;
        }

        public bool DelKey(string path)
        {
            try
            {
                _rootKey.DeleteSubKeyTree(path, true);
                _rootKey.Close();
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }

        }

        public bool IsKeyExist(string path, string key)
        {
            var keyNames = this.OpenKey(path).GetSubKeyNames();
            bool flag = keyNames.Contains(key);
            _rootKey.Close();
            return flag;
        }

        public void SetValue(string path, string name, string val)
        {
            RegistryKey key = this.OpenKey(path, true);
            key.SetValue(name, val);
            _rootKey.Close();
        }

        public string GetValue(string path, string name)
        {
            var key = _rootKey.OpenSubKey(path);
            string val = key.GetValue(name).ToString();
            return val;
        }

        public bool DelValue(string path, string name)
        {
            bool flag;
            try
            {
                var key = this.OpenKey(path, true);
                key.DeleteValue(name, true);
                key.Close();
                flag = true;
            }
            catch (Exception exp)
            {
                flag = false;
            }
            return flag;
        }

        public bool IsValueExist(string path, string name)
        {
            var valueNames = this.OpenKey(path).GetValueNames();
            bool flag = valueNames.Contains(name);
            _rootKey.Close();
            return flag;
        }
    }
}
