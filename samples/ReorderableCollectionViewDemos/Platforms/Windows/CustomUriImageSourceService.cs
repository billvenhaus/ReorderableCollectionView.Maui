#nullable enable
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Media.Imaging;
using WImageSource = Microsoft.UI.Xaml.Media.ImageSource;

namespace ReorderableCollectionViewDemos
{
	// The default UriImageSourceService handler is inconsistent & doesn't always display the images.
	public class CustomUriImageSourceService : UriImageSourceService
	{
		public override Task<IImageSourceServiceResult<WImageSource>?> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default) =>
			GetCustomImageSourceAsync((IUriImageSource)imageSource, scale, cancellationToken);

		Task<IImageSourceServiceResult<WImageSource>?> GetCustomImageSourceAsync(IUriImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
		{
			var imageLoader = imageSource;

			if (imageLoader?.Uri == null)
				return null;

			var image = new BitmapImage();
			image.UriSource = imageLoader.Uri;

			var result = new ImageSourceServiceResult(image);

			return Task.FromResult((IImageSourceServiceResult<WImageSource>?)result);
		}
	}
}