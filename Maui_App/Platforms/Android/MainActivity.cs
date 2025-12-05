using Android.App;
using Android.Content.PM;
using Microsoft.Maui.Controls.Platform; // Corrected using statement

namespace Maui_App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                         ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity { }