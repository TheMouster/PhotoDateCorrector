﻿using System;
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

		public ProgressDialog(String[] filePaths) : this()
		{
			//progressBar.Minimum = 0;
			//progressBar.Maximum = 100;
			backgroundWorker.RunWorkerAsync(filePaths);
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			String[] filePaths = (String[])e.Argument;

			int numberProcessed = 1, progressPercentage = 0;
			int numberToProcess = filePaths.Length;

			foreach(var path in filePaths)
			{
				if (backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				PhotoProcessor.ProcessImage(path);
				progressPercentage = (numberProcessed++ * 100) / filePaths.Length;
				backgroundWorker.ReportProgress(progressPercentage);
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar.Value = e.ProgressPercentage;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Close(); //Close the form. We're done.
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			backgroundWorker.CancelAsync();
		}
	}
}
