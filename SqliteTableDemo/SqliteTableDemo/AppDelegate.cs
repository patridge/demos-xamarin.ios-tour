using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Maps;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Path = System.IO.Path;

namespace SqliteTableDemo {
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate {
        UIWindow window;
        UINavigationController _Nav;
        SqliteTableDemoViewController _RootVC;
        const string MapsApiKey = "{put your Maps API key here}";

        // * Demo of SQLite.NET, Google Maps[, and Progress HUD (after question from audience)].
        // * NOTE: Requires paid license to run; Google Maps component is enormous.

        public static string DatabaseFilePath {
            get { 
                var sqliteFilename = "MarkerInfoDb.db3";
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine(documentsPath, "../Library/"); // Library folder
                var path = Path.Combine(libraryPath, sqliteFilename);
                return path;    
            }
        }
        public static MarkerInfoDatabase DemoDatabase {
            get {
                return new MarkerInfoDatabase(DatabaseFilePath);
            }
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            _RootVC = new SqliteTableDemoViewController(DemoDatabase);
            _Nav = new UINavigationController(_RootVC);
            window.RootViewController = _Nav;
            window.MakeKeyAndVisible();

            MapServices.ProvideAPIKey(MapsApiKey);

            return true;
        }
    }
}

