using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExpressBase.Mobile.Constants;
using ExpressBase.Mobile.Helpers;
using ExpressBase.Mobile.Services;
using Foundation;
using UIKit;

namespace ExpressBase.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            App _app = null;

            string sid = Store.GetValue(AppConst.SID);

            if (string.IsNullOrEmpty(sid))
            {
                _app = new App();
            }
            else
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), string.Format("{0}.db3", sid));

                if (!File.Exists(dbPath))
                {
                    Mono.Data.Sqlite.SqliteConnection.CreateFile(dbPath);
                }

                _app = new App(dbPath);
            }

            LoadApplication(_app);

            return base.FinishedLaunching(app, options);
        }
    }
}
