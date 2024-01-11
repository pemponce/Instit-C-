using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace kg
{
    public partial class MainWindow : Window
    {
        private BitmapImage originalImage;
        private WriteableBitmap processedImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                originalImage = new BitmapImage(new Uri(openFileDialog.FileName));
                OriginalImage.Source = originalImage;
            }
        }

        private void ProcessImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Пожалуйста, сначала загрузите изображение!", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            processedImage = null;

            switch (MethodComboBox.SelectedIndex)
            {
                case 0:
                    processedImage = PreprocessingMethod(originalImage, 0, 0, (int)originalImage.Width,
                        (int)originalImage.Height);
                    break;
                case 1:
                    processedImage = LinearContrastMethod(originalImage, 1.0);
                    break;
                case 2:
                    processedImage = SolarizationMethod(originalImage, 128);
                    break;
                case 3:
                    processedImage = LogarithmicBrightnessMethod(originalImage);
                    break;
            }

            ProcessedImage.Source = processedImage;
        }

        private WriteableBitmap PreprocessingMethod(BitmapImage originalImage, int startX, int startY, int width,
            int height)
        {
            if (originalImage == null)
            {
                return null;
            }

            FormatConvertedBitmap formattedBitmap =
                new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            byte[] pixels = new byte[4 * (int)originalImage.Width * (int)originalImage.Height];
            formattedBitmap.CopyPixels(new Int32Rect(0, 0, (int)originalImage.Width, (int)originalImage.Height), pixels,
                4 * (int)originalImage.Width, 0);

            int stride = 4 * width; // ширина строки
            byte[] croppedPixels = new byte[stride * height];

            for (int i = 0; i < height; i++)
            {
                Array.Copy(pixels, (startY + i) * stride + startX * 4, croppedPixels, i * stride, stride);
            }

            WriteableBitmap resultBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            resultBitmap.WritePixels(new Int32Rect(0, 0, width, height), croppedPixels, stride, 0);

            return resultBitmap;
        }

        private WriteableBitmap LinearContrastMethod(BitmapImage originalImage, double contrastFactor)
        {
            FormatConvertedBitmap formattedBitmap =
                new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            byte[] pixels = new byte[4 * (int)(originalImage.PixelWidth * originalImage.PixelHeight)];
            int stride = 4 * (int)originalImage.PixelWidth;

            formattedBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                int red = (int)(contrastFactor * (pixels[i + 2] - 128) + 128);
                int green = (int)(contrastFactor * (pixels[i + 1] - 128) + 128);
                int blue = (int)(contrastFactor * (pixels[i] - 128) + 128);
                red = Math.Max(0, Math.Min(255, red));
                green = Math.Max(0, Math.Min(255,
                    blue = Math.Max(0, Math.Min(255, blue))));
                pixels[i + 2] = (byte)red;
                pixels[i + 1] = (byte)green;
                pixels[i] = (byte)blue;
            }

            WriteableBitmap resultBitmap = new WriteableBitmap(formattedBitmap);
            resultBitmap.WritePixels(new Int32Rect(0, 0, (int)originalImage.PixelWidth, (int)originalImage.PixelHeight),
                pixels, stride, 0);

            return resultBitmap;
        }

        private WriteableBitmap SolarizationMethod(BitmapImage originalImage, int threshold)
        {
            FormatConvertedBitmap formattedBitmap =
                new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            byte[] pixels = new byte[4 * (int)(originalImage.PixelWidth * originalImage.PixelHeight)];
            int stride = 4 * (int)originalImage.PixelWidth;

            formattedBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                int red = pixels[i + 2] > threshold ? 255 - pixels[i + 2] : pixels[i + 2];
                int green = pixels[i + 1] > threshold ? 255 - pixels[i + 1] : pixels[i + 1];
                int blue = pixels[i] > threshold ? 255 - pixels[i] : pixels[i];

                pixels[i + 2] = (byte)red;
                pixels[i + 1] = (byte)green;
                pixels[i] = (byte)blue;
            }

            WriteableBitmap resultBitmap = new WriteableBitmap(formattedBitmap);
            resultBitmap.WritePixels(new Int32Rect(0, 0, (int)originalImage.PixelWidth, (int)originalImage.PixelHeight),
                pixels, stride, 0);

            return resultBitmap;
        }

        private WriteableBitmap LogarithmicBrightnessMethod(BitmapImage originalImage)
        {
            FormatConvertedBitmap formattedBitmap =
                new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            byte[] pixels = new byte[4 * (int)(originalImage.PixelWidth * originalImage.PixelHeight)];
            int stride = 4 * (int)originalImage.PixelWidth;

            formattedBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                int red = (int)(255 * Math.Log(1 + pixels[i + 2], 256));
                int green = (int)(255 * Math.Log(1 + pixels[i + 1], 256));
                int blue = (int)(255 * Math.Log(1 + pixels[i], 256));
                red = Math.Max(0, Math.Min(255, red));
                green = Math.Max(0, Math.Min(255, green));
                blue = Math.Max(0, Math.Min(255, blue));

                pixels[i + 2] = (byte)red;
                pixels[i + 1] = (byte)green;
                pixels[i] = (byte)blue;
            }

            WriteableBitmap resultBitmap = new WriteableBitmap(formattedBitmap);
            resultBitmap.WritePixels(new Int32Rect(0, 0, (int)originalImage.PixelWidth, (int)originalImage.PixelHeight),
                pixels, stride, 0);

            return resultBitmap;
        }
    }
}