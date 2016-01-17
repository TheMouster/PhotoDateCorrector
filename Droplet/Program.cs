using PhotoDateCorrector;
using System;
using System.Windows.Forms;

namespace Droplet
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(String[] filePaths)
		{
			if (filePaths.Length < 1)
			{
				DisplayInstructions();
				return;
			}

			//If there's only 1 file don't bother to display the progress dialog. 
			if (filePaths.Length == 1)
				PhotoProcessor.ProcessImage(filePaths[0]);
			else
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new ProgressDialog(filePaths));
			}
		}

		/// <summary>
		/// Displays the instructions.
		/// </summary>
		private static void DisplayInstructions()
		{
			MessageBox.Show("Drop images with embedded EXIF tags on me. Where possible I'll update the images' DateTime, DateTimeOriginal, ThumbnailDateTime & File Creation Date to match the EXIF DateDigitized tag.");
		}
	}
}
