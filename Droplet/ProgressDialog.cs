using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PhotoDateCorrector
{
	public partial class ProgressDialog : Form
	{
		public ProgressDialog()
		{
			InitializeComponent();
		}

		public ProgressDialog(String[] filePaths)
		{
			progressBar.Minimum = 0;
			progressBar.Maximum = 100;
			backgroundWorker.RunWorkerAsync(filePaths);
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			String[] filePaths = (String[])e.Argument;

			int numberProcessed = 0;
			int numberToProcess = filePaths.Length;

			foreach(var path in filePaths)
			{
				if (backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				PhotoProcessor.ProcessImage(path);
				numberProcessed++;
				backgroundWorker.ReportProgress(numberProcessed * 100 / filePaths.Length);
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar.Value = e.ProgressPercentage;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//Unused.
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			backgroundWorker.CancelAsync();
		}
	}
}
