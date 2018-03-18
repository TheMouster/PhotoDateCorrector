using ExifLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PhotoDateCorrector
{
    public static class PhotoProcessor
	{
        const String APPLICATION_LOG = "Application";

        public static void ProcessImages(String[] filePaths)
		{
			Parallel.ForEach(filePaths, (currentFile) =>
			{
				ProcessImage(currentFile);
			});
		}

		public static void ProcessImage(String filePath)
		{
            ImageFile image;

            try
            {
                image = ImageFile.FromFile(filePath);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(APPLICATION_LOG, FormatMessage(ex.Message), EventLogEntryType.Error);
                return;
            }

            DateTime whenDigitised = DateTime.MinValue;
			try
			{
				whenDigitised = (DateTime)image.Properties[ExifTag.DateTimeDigitized].Value;
			}
			catch(KeyNotFoundException kex) //EXIF data doesn't contain a DateTimeDigitized flag
			{
                EventLog.WriteEntry(APPLICATION_LOG, FormatMessage("Likely missing DateTimeDigitized EXIF tag." + kex.Message), EventLogEntryType.Error);
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

            var correctedGPSDate = new DateTime(whenDigitised.Year, whenDigitised.Month, whenDigitised.Day);
            try
            {
                var gpsDate = (DateTime)image.Properties[ExifTag.GPSDateStamp].Value;
                if (gpsDate == DateTime.MinValue)
                {
                    image.Properties[ExifTag.GPSDateStamp].Value = correctedGPSDate;
                }
            }
            catch (KeyNotFoundException)
            {
                var gpsDate = new ExifDateTime(ExifTag.GPSDateStamp, correctedGPSDate);
                image.Properties.Add(ExifTag.GPSDateStamp,gpsDate);
            }

            try
            {
                image.Save(filePath);

                //Update the images' file creation time.
                File.SetCreationTime(filePath, whenDigitised);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(APPLICATION_LOG, FormatMessage(ex.Message), EventLogEntryType.Error);
            }
        }

        private static String FormatMessage(String message)
        {
            return "PhotoDateCorrector: " + message;
        }
	}
}
