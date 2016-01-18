using System;
using System.IO;
using ExifLibrary;

namespace PhotoDateCorrector
{
	public static class PhotoProcessor
	{
		public static void ProcessImage(String filePath)
		{
			ExifFile image = ExifFile.Read(filePath);
			DateTime whenDigitised = (DateTime)image.Properties[ExifTag.DateTimeDigitized].Value;

			image.Properties[ExifTag.DateTime].Value = whenDigitised;
			image.Properties[ExifTag.DateTimeOriginal].Value = whenDigitised;

			//image.Save(filePath);

			return;
		}
	}
}
