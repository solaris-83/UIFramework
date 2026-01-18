

using System;
using System.IO;

namespace UIFrameworkDotNet.Helpers
{
    public static class ImageHelper
    {
        public static string ConvertImageToBase64(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            // Get the file extension to determine the MIME type
            string extension = Path.GetExtension(imagePath).ToLower();
            string mimeType;
            if (extension == ".jpg" || extension == ".jpeg")
            {
                mimeType = "image/jpeg";
            }
            else if (extension == ".png")
            {
                mimeType = "image/png";
            }
            else if (extension == ".gif")
            {
                mimeType = "image/gif";
            }
            else if (extension == ".bmp")
            {
                mimeType = "image/bmp";
            }
            else if (extension == ".webp")
            {
                mimeType = "image/webp";
            }
            else
            {
                throw new NotSupportedException("Unsupported image format.");
            }

            return $"data:{mimeType};base64,{base64String}";
        }
    }
}
