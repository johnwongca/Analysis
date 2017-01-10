using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using METALIBLib;

namespace ToMetaStock
{
	public partial class frmMain : Form
	{
		string folder, symbol;
		public frmMain()
		{
			InitializeComponent();
		}

		private void btnReadFolder_Click(object sender, EventArgs e)
		{
			fBrowserDialog.SelectedPath = tbRead_Folder.Text;
			if (fBrowserDialog.ShowDialog(this) == DialogResult.OK)
			{
				tbRead_Folder.Text = fBrowserDialog.SelectedPath;
			}
			else
				return;
			if (tbRead_Folder.Text == "")
				return;
			if (MessageBox.Show("Read from selected path?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
				btnRefresh_Click(null, null);
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			folder = tbRead_Folder.Text;
			if (tbRead_Folder.Text == "")
				return;
			MLReaderClass lReader = new MLReaderClass();
			try
			{
				lvSecPrices.Items.Clear();
				lvSecurity.Items.Clear();
				lReader.OpenDirectory(folder);
				while (lReader.iMaRecordsLeft > 0)
				{
					lReader.ReadMaster();
					ListViewItem item = lvSecurity.Items.Add(lReader.sMaSecSymbol);
					item.SubItems.Add(lReader.sMaSecName);
					item.SubItems.Add(lReader.iMaFirstDate + " " + lReader.sMaStartTime);
					item.SubItems.Add(lReader.iMaLastDate+ " " + lReader.sMaEndTime);
					item.SubItems.Add(lReader.MaPeriodicity.ToString());
					item.SubItems.Add(lReader.MaInterval.ToString());
					item.SubItems.Add(lReader.iRound.ToString());
					item.SubItems.Add(lReader.iMaNrFields.ToString());
					item.SubItems.Add(lReader.sMaSecFileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				lReader.CloseDirectory();
			}
		}

		private void GetSecurities(string security)
		{
			symbol = security;
			METALIBLib.MLReaderClass lReader = new METALIBLib.MLReaderClass();

			int iDate = 0;
			int iRecords = 0;
			double dOpen = 0, dHigh = 0, dLow = 0, dClose = 0, dVolume = 0;

			lvSecPrices.Items.Clear();

			try
			{
				lReader.OpenDirectory(folder);
				lReader.OpenSecurityBySymbol(security); // Open selected security

				while (lReader.iSeRecordsLeft > 0)
				{
					lReader.ReadDay();

					iDate = lReader.iSeDate;
					dOpen = lReader.dSeOpen;
					dHigh = lReader.dSeHigh;
					dLow = lReader.dSeLow;
					dClose = lReader.dSeClose;
					dVolume = lReader.dSeVolume;

					ListViewItem lItem = lvSecPrices.Items.Insert(iRecords, iDate.ToString());
					lItem.SubItems.Add(dOpen.ToString());
					lItem.SubItems.Add(dHigh.ToString());
					lItem.SubItems.Add(dLow.ToString());
					lItem.SubItems.Add(dClose.ToString());
					lItem.SubItems.Add(dVolume.ToString());

					iRecords++;
				}
			}
			catch (System.Runtime.InteropServices.COMException ComEx)
			{
				MessageBox.Show(ComEx.Message);
			}
			finally
			{
				lReader.CloseDirectory();
			}    
		}

		private void lvSecurity_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lvSecurity.SelectedItems.Count == 0)
				return;
			GetSecurities(lvSecurity.SelectedItems[0].Text);
		}

		private void btnExportAll_Click(object sender, EventArgs e)
		{
			string folder = tbWrite_Folder.Text;
			int daysBack = Decimal.ToInt32(nDaysBack.Value);
			EventHandler evn = new EventHandler(OnProgress);
			ExportExhange tt;
			SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=Stock;Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True");
			cn.Open();
			SqlDataAdapter da = new SqlDataAdapter("select Name from STK.Exchange order by Name", cn);
			DataTable dt = new DataTable();
			da.Fill(dt);
			cn.Close();
			foreach (DataRow r in dt.Rows)
			{
				tt = new ExportExhange(folder, r[0].ToString(), daysBack, ckSort.Checked);
				tt.OnProgress += evn;
				tt.Export();
				tt.OnProgress -= evn;
				tt = null;
				GC.Collect();
			}
		}
		private void OnProgress(object sender, EventArgs e)
		{
			l1.Text = ((ExportExhange)sender).ExchangeName+ " - " + ((ExportExhange)sender).Symbol;
			p1.Value = ((ExportExhange)sender).Percent;
			if (((ExportExhange)sender).Error != null)
				textBox1.Text = textBox1.Text + Environment.NewLine + ((ExportExhange)sender).Error.ToString() + Environment.NewLine + ((ExportExhange)sender).ExchangeName + " - " + ((ExportExhange)sender).Symbol;
			Application.DoEvents();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			fBrowserDialog.SelectedPath = tbWrite_Folder.Text;
			if (fBrowserDialog.ShowDialog(this) == DialogResult.OK)
			{
				tbWrite_Folder.Text = fBrowserDialog.SelectedPath;
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{

		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			ExportExhange tt = new ExportExhange("d:\\test", "NASDAQ", 3, false);
			tt.Export();
		}

		private void cmSecurity_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = lvSecurity.SelectedItems.Count == 0;
		}

		private void cmPrice_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = lvSecPrices.SelectedItems.Count == 0;
		}

		private void miDeleteSecurity_Click(object sender, EventArgs e)
		{
			MLWriter m_writer = new MLWriter();
			try
			{
				foreach (ListViewItem i in lvSecurity.SelectedItems)
				{
					try
					{
						m_writer.OpenDirectory(folder);
						m_writer.DeleteSecurity(i.Text);
					}
					catch { }
					finally
					{
						try { m_writer.CloseDirectory(); }
						catch { }
					}
				}
			}
			finally
			{
				m_writer = null;
				GC.Collect();
				tbRead_Folder.Text = folder;
				btnRefresh_Click(null, null);
			}
		}

		private void miDeletePrices_Click(object sender, EventArgs e)
		{
			MLWriter m_writer = new MLWriter();
			try
			{
				try
				{
					m_writer.OpenDirectory(folder);
					m_writer.OpenSecurityBySymbol(symbol);
					foreach (ListViewItem i in lvSecPrices.SelectedItems)
					{
						m_writer.DeleteSecRecord(Int32.Parse(i.Text));
					}
					
				}
				catch (Exception ee) { MessageBox.Show(ee.ToString()); }
				finally
				{
					try { m_writer.CloseSecurity(); }
					catch { }
					try { m_writer.CloseDirectory(); }
					catch { }
				}
			}
			finally
			{
				m_writer = null;
				GetSecurities(symbol);
				GC.Collect();
			}
		}
	}
}