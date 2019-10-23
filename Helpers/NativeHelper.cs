using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ExpressBase.Mobile.iOS.Helpers;
using ExpressBase.Mobile.Models;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
namespace ExpressBase.Mobile.iOS.Helpers
{
    public class NativeHelper : INativeHelper
    {
        public void CloseApp()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}