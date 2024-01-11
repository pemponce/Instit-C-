using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace kg3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(filePath);
                byte[] compressedData = CompressImage(imageData);
                double compressionRatio = (double)compressedData.Length / imageData.Length;

                string compressedFilePath = Path.ChangeExtension(filePath, "_compressed.bmp");
                File.WriteAllBytes(compressedFilePath, compressedData);

                MessageBox.Show($"Compression complete! Compression ratio: {compressionRatio:F2}\nCompressed image saved to: {compressedFilePath}");
            }
        }

        private byte[] CompressImage(byte[] imageData)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                int count = 1;
                for (int i = 1; i < imageData.Length; i++)
                {
                    if (imageData[i] == imageData[i - 1])
                    {
                        count++;
                    }
                    else
                    {
                        compressedStream.WriteByte((byte)count);
                        compressedStream.WriteByte(imageData[i - 1]);
                        count = 1;
                    }
                }

                compressedStream.WriteByte((byte)count);
                compressedStream.WriteByte(imageData[imageData.Length - 1]);

                return compressedStream.ToArray();
            }
        }
    }
}
