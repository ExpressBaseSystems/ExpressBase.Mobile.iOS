using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressBase.Mobile.Enums;
using ExpressBase.Mobile.iOS.Helpers;
using ExpressBase.Mobile.Models;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace ExpressBase.Mobile.iOS.Helpers
{
    public class NativeHelper : INativeHelper
    {
        public string AppVersion => NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();

        public string DeviceId => UIDevice.CurrentDevice.IdentifierForVendor.AsString();

        public string NativeRoot => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public void CloseApp()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }

        public bool DirectoryOrFileExist(string DirectoryPath, SysContentType Type)
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

        public string CreateDirectoryOrFile(string DirectoryPath, SysContentType Type)
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

        public byte[] GetPhoto(string url)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), url);
            return File.ReadAllBytes(path);
        }
    }

    public class ToastMessage : IToast
    {
        const double LONG_DELAY = 3.5;

        NSTimer alertDelay;
        UIAlertController alert;

        public void Show(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }


        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}