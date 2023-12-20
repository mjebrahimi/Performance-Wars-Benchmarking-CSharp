using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SkiaSharp;

namespace Utilities;

public static class ImageCompressor
{
    public static class ImageSharp
    {
        /// <summary>
        /// Compresses a JPEG image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="quality">
        /// Gets the quality, that will be used to encode the image. Quality
        /// index must be between 1 and 100 (compression from max to min).
        /// Defaults to <value>75</value>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task CompressJpegAsync(string path, int quality = 75, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var image = await Image.LoadAsync(path, cancellationToken);
            var encoder = new JpegEncoder { Quality = quality };
            await image.SaveAsync(path, encoder, cancellationToken);
        }

        /// <summary>
        /// Compresses a PNG image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task CompressPngAsync(string path, PngCompressionLevel compressionLevel = PngCompressionLevel.BestCompression, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            using var image = await Image.LoadAsync(path, cancellationToken);
            var encoder = new PngEncoder { CompressionLevel = compressionLevel };
            await image.SaveAsync(path, encoder, cancellationToken);
        }

        /// <summary>
        /// Compresses a WEBP image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="quality">
        /// Gets the compression quality. Between 0 and 100. (0: no compression, 100: max compression)
        /// For lossy, 0 gives the smallest size and 100 the largest. For lossless,
        /// this parameter is the amount of effort put into the compression: 0 is the fastest but gives larger
        /// files compared to the slowest, but best, 100.
        /// Defaults to <value>75</value>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task CompressWebpAsync(string path, int quality = 75, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var image = await Image.LoadAsync(path, cancellationToken);
            var encoder = new WebpEncoder { Quality = quality };
            await image.SaveAsync(path, encoder, cancellationToken);
        }

        /// <summary>
        /// Compresses a JPEG image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="quality">
        /// Gets the quality, that will be used to encode the image. Quality
        /// index must be between 1 and 100 (compression from max to min).
        /// Defaults to <value>75</value>.
        /// </param>
        public static void CompressJpeg(string path, int quality = 75)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var image = Image.Load(path);
            var encoder = new JpegEncoder { Quality = quality };
            image.Save(path, encoder);
        }

        /// <summary>
        /// Compresses a PNG image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public static void CompressPng(string path, PngCompressionLevel compressionLevel = PngCompressionLevel.BestCompression)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            using var image = Image.Load(path);
            var encoder = new PngEncoder { CompressionLevel = compressionLevel };
            image.Save(path, encoder);
        }

        /// <summary>
        /// Compresses a WEBP image.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="quality">
        /// Gets the compression quality. Between 0 and 100. (0: no compression, 100: max compression)
        /// For lossy, 0 gives the smallest size and 100 the largest. For lossless,
        /// this parameter is the amount of effort put into the compression: 0 is the fastest but gives larger
        /// files compared to the slowest, but best, 100.
        /// Defaults to <value>75</value>.
        /// </param>
        public static void CompressWebp(string path, int quality = 75)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var image = Image.Load(path);
            var encoder = new WebpEncoder { Quality = quality };
            image.Save(path, encoder);
        }

        /// <summary>
        /// Compresses a JPEG image.
        /// </summary>
        /// <param name="bytes">The bytes of the image.</param>
        /// <param name="quality">
        /// Gets the quality, that will be used to encode the image. Quality
        /// index must be between 1 and 100 (compression from max to min).
        /// Defaults to <value>75</value>.
        /// </param>
        /// <returns></returns>
        public static byte[] CompressJpeg(byte[] bytes, int quality = 75)
        {
            EnumerableGuard.ThrowIfNullOrEmpty(bytes);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            var inputStream = new MemoryStream(bytes);
            using var image = Image.Load(inputStream);
            var encoder = new JpegEncoder { Quality = quality };
            var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            return outputStream.GetTrimmedBuffer();
        }

        /// <summary>
        /// Compresses a PNG image.
        /// </summary>
        /// <param name="bytes">The bytes of the image.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns></returns>
        public static byte[] CompressPng(byte[] bytes, PngCompressionLevel compressionLevel = PngCompressionLevel.BestCompression)
        {
            EnumerableGuard.ThrowIfNullOrEmpty(bytes);

            var inputStream = new MemoryStream(bytes);
            using var image = Image.Load(inputStream);
            var encoder = new PngEncoder { CompressionLevel = compressionLevel };
            var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            return outputStream.GetTrimmedBuffer();
        }

        /// <summary>
        /// Compresses a WEBP image.
        /// </summary>
        /// <param name="bytes">The bytes of the image.</param>
        /// <param name="quality">
        /// Gets the compression quality. Between 0 and 100. (0: no compression, 100: max compression)
        /// For lossy, 0 gives the smallest size and 100 the largest. For lossless,
        /// this parameter is the amount of effort put into the compression: 0 is the fastest but gives larger
        /// files compared to the slowest, but best, 100.
        /// Defaults to <value>75</value>.
        /// </param>
        /// <returns></returns>
        public static byte[] CompressWebp(byte[] bytes, int quality = 75)
        {
            EnumerableGuard.ThrowIfNullOrEmpty(bytes);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            var inputStream = new MemoryStream(bytes);
            using var image = Image.Load(inputStream);
            var encoder = new WebpEncoder { Quality = quality };
            var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            return outputStream.GetTrimmedBuffer();
        }
    }

    public static class SkiaSharp
    {
        /// <summary>
        /// Compresses the JPEG.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="quality">The quality.</param>
        public static void CompressJpeg(string path, int quality = 80)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(path);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Jpeg, quality);
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            compressedData.SaveTo(fileStream);
            fileStream.Flush();
        }

        /// <summary>
        /// Compresses the PNG.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="quality">The quality.</param>
        public static void CompressPng(string path, int quality = 80)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(path);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Png, quality);
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            compressedData.SaveTo(fileStream);
            fileStream.Flush();
        }

        /// <summary>
        /// Compresses the webp.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="quality">The quality.</param>
        public static void CompressWebp(string path, int quality = 80)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(path);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Webp, quality);
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            compressedData.SaveTo(fileStream);
            fileStream.Flush();
        }

        /// <summary>
        /// Compresses the JPEG.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static byte[] CompressJpeg(ReadOnlySpan<byte> bytes, int quality = 80)
        {
            if (bytes.Length == 0) throw new ArgumentException("Argument is empty.", nameof(bytes));
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(bytes);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Jpeg, quality);
            return compressedData.ToArray(); //compressedData.Span;
        }

        /// <summary>
        /// Compresses the PNG.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static byte[] CompressPng(ReadOnlySpan<byte> bytes, int quality = 80)
        {
            if (bytes.Length == 0) throw new ArgumentException("Argument is empty.", nameof(bytes));
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(bytes);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Png, quality);
            return compressedData.ToArray();
        }

        /// <summary>
        /// Compresses the webp.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static byte[] CompressWebp(ReadOnlySpan<byte> bytes, int quality = 80)
        {
            if (bytes.Length == 0) throw new ArgumentException("Argument is empty.", nameof(bytes));
            RangeGuard.ThrowIfNotInRange(quality, 0, 100);

            using var bitmap = SKBitmap.Decode(bytes);
            using var compressedData = bitmap.Encode(SKEncodedImageFormat.Webp, quality);
            return compressedData.ToArray();
        }
    }
}
