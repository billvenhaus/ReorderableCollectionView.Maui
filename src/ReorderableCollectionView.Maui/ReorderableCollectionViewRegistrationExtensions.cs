using System;
using Microsoft.Maui.Hosting;

namespace ReorderableCollectionView.Maui
{
	public static class ReorderableCollectionViewRegistrationExtensions
	{
		public static MauiAppBuilder RegisterReorderableCollectionView(this MauiAppBuilder appHostBuilder)
		{
			if (appHostBuilder is null)
			{
				throw new ArgumentNullException(nameof(appHostBuilder));
			}

			appHostBuilder.ConfigureMauiHandlers(handlers => handlers.AddHandler<ReorderableCollectionView, ReorderableCollectionViewHandler>());

			return appHostBuilder;
		}
	}
}