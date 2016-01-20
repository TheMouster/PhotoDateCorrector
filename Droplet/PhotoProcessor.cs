using System;
using System.IO;
using ExifLibrary;
using System.Collections.Generic;

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
			DateTime whenDigitised = DateTime.MinValue;

			try
			{
				whenDigitised = (DateTime)image.Properties[ExifTag.DateTimeDigitized].Value;
			}
			catch(KeyNotFoundException) //EXIF data doesn't contain a DateTimeDigitized flag
			{
				return;
			}
			
			//Correct the EXIF values in the image and save it.
			//TODO: Deal with the situation where tags don't exist and need to be created. I'm looking at you
			//ThumbnailDateTime tag.
			try
			{
				image.Properties[ExifTag.DateTime].Value = whenDigitised;
			}
			catch(KeyNotFoundException)
			{
			}

			try
			{
				image.Properties[ExifTag.DateTimeOriginal].Value = whenDigitised;
			}
			catch (KeyNotFoundException)
			{
			}

			try
			{
				image.Properties[ExifTag.ThumbnailDateTime].Value = whenDigitised;
			}
			catch (KeyNotFoundException)
			{
			}

			image.Save(filePath);
			
			//Update the images' file creation time.
			File.SetCreationTime(filePath, whenDigitised);
		}
	}
}
