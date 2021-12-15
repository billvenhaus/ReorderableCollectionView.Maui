using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ReorderableCollectionViewDemos
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}