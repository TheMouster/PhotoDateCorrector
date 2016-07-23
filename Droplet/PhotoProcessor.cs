using System;
using System.IO;
using ExifLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoDateCorrector
{
	public static class PhotoProcessor
	{
		public static void ProcessImages(String[] filePaths)
		{
			/*
			foreach (var path in filePaths)
			{
				ProcessImage(path);
			}
			*/

			Parallel.ForEach(filePaths, (currentFile) =>
			{
				ProcessImage(currentFile);
			});
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
			try
			{
				image.Properties[ExifTag.DateTime].Value = whenDigitised;
			}
			catch(KeyNotFoundException) //No DateTime? Add one.
			{
				var dateTime = new ExifDateTime(ExifTag.DateTime, whenDigitised);
				image.Properties.Add(ExifTag.DateTime, dateTime);
			}

			try
			{
				image.Properties[ExifTag.DateTimeOriginal].Value = whenDigitised;
			}
			catch (KeyNotFoundException) //No DateTimeOriginal? Add one.
			{
				var dateTimeOriginal = new ExifDateTime(ExifTag.DateTimeOriginal, whenDigitised);
				image.Properties.Add(ExifTag.DateTimeOriginal, dateTimeOriginal);
			}

			try
			{
				image.Properties[ExifTag.ThumbnailDateTime].Value = whenDigitised;
			}
			catch (KeyNotFoundException) //No ThumbnailDateTime? Add one.
			{
				var thumbnailDate = new ExifDateTime(ExifTag.ThumbnailDateTime, whenDigitised);
				image.Properties.Add(ExifTag.ThumbnailDateTime, thumbnailDate);
			}

			image.Save(filePath);
			
			//Update the images' file creation time.
			File.SetCreationTime(filePath, whenDigitised);
		}
	}
}
