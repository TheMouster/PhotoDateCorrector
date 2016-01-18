using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhotoDateCorrector;

namespace Droplet
{
	public partial class ProgressDialog : Form
	{
		BackgroundWorker backgroundWorker;

		public ProgressDialog()
		{
			InitializeComponent();
		}

		public ProgressDialog(String[] filePaths)
		{
			foreach (var path in filePaths)
			{
				PhotoProcessor.ProcessImage(path);
			}
		}
	}
}
