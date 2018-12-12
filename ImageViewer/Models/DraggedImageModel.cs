using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageViewer.Models
{
    public class DraggedImageModel
    {
        private readonly List<string> _supportedImageFormats;

        public DraggedImageModel()
        {
            _supportedImageFormats = new List<string>
            {
                ".jpg",
                ".jpeg",
                ".png"
            };
        }

        public bool IsDraggedFileImage(object parameter)
        {
            var imagePaths = ToArrayOfFilePaths(parameter);
            return imagePaths.Length != 0 &&
                   imagePaths.Any(imagePath => _supportedImageFormats.Contains(Path.GetExtension(imagePath)?.ToLower()));
        }

        public IEnumerable<string> GetDroppedImagesPaths(object parameter)
        {
            var imagePaths = ToArrayOfFilePaths(parameter);
            var filteredImagePaths = FilterNotSupportedFormats(imagePaths);

            foreach (var imagePath in filteredImagePaths)
            {
                yield return imagePath;
            }
        }

        private string[] ToArrayOfFilePaths(object givenParameter)
        {
            return givenParameter as string[] ?? new string[] { };
        }

        private string[] FilterNotSupportedFormats(string[] filePaths)
        {
            return filePaths.Where(filePath => _supportedImageFormats.Contains(Path.GetExtension(filePath)?.ToLower())).ToArray();
        }
    }
}