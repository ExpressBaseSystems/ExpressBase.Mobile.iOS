using System;
using System.Diagnostics;
using System.IO;
using ExpressBase.Mobile.Enums;
using ExpressBase.Mobile.Helpers;
using ExpressBase.Mobile.iOS.Helpers;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
namespace ExpressBase.Mobile.iOS.Helpers
{
    public class NativeHelper : INativeHelper
    {
        public string AppVersion => NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();

        public string DeviceId => UIDevice.CurrentDevice.IdentifierForVendor.AsString();

        public string NativeRoot => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public void Close()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }

        public bool Exist(string DirectoryPath, SysContentType Type)
        {
            try
            {
                var pathToNewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DirectoryPath);
                if (Type == SysContentType.File)
                {
                    return File.Exists(pathToNewFolder);
                }
                else if (Type == SysContentType.Directory)
                {
                    return Directory.Exists(pathToNewFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public string Create(string DirectoryPath, SysContentType Type)
        {
            try
            {
                var pathToNewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DirectoryPath);
                if (Type == SysContentType.Directory)
                {
                    Directory.CreateDirectory(pathToNewFolder);
                }
                else
                {
                    File.Create(pathToNewFolder);
                }
                return pathToNewFolder;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public byte[] GetFile(string url)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), url);
                return File.ReadAllBytes(path);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            return null;
        }

        public string[] GetFiles(string Url, string Pattern)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Url);
            return Directory.GetFiles(path, Pattern);
        }

        public string GetAssetsURl()
        {
            return NSBundle.MainBundle.BundlePath;
        }

        public void WriteLogs(string message, LogTypes logType)
        {
            try
            {
                string sid = App.Settings.Sid.ToUpper();
                string root = App.Settings.AppDirectory;

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"/{root}/{sid}/logs.txt");

                // Create a string array with the additional lines of text
                string[] lines = {
                    $"CREATED ON { DateTime.UtcNow }",
                    $"{logType} : {message}"
                };

                File.AppendAllLines(path, lines);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }
    }
}