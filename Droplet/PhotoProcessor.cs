using System;
using System.IO;
using ExifLibrary;

namespace PhotoDateCorrector
{
	public static class PhotoProcessor
	{
		public static void ProcessImages(String[] filePaths)
		{
			foreach(var path in filePaths)
			{
				ProcessImage(path);
			}
		}

		public static void ProcessImage(String filePath)
		{
			ExifFile image = ExifFile.Read(filePath);
			DateTime whenDigitised = (DateTime)image.Properties[ExifTag.DateTimeDigitized].Value;

			//Correct the EXIF values in the image and save it.
			image.Properties[ExifTag.DateTime].Value = whenDigitised;
			image.Properties[ExifTag.DateTimeOriginal].Value = whenDigitised;
			image.Properties[ExifTag.ThumbnailDateTime].Value = whenDigitised;
			image.Save(filePath);

			//Update the images' file creation time.
			File.SetCreationTime(filePath, whenDigitised);
		}
	}
}
