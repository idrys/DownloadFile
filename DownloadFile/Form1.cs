using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        string[] urls = {
            "http://www.cersanit.com.pl/public/B_Cersanit_12_2016.exe",
            "http://opoczno.eu/uploads/CAD_Decor_Opoczno_3D_2016-08-17.exe",
            "http://www.cersanit.com.pl/public/plytki_cersanit_08_2017.exe",
            "http://opoczno.eu/uploads/plytki_opoczno.exe"

        };
        

        private void button1_Click(object sender, EventArgs e)
        {

            
            FireDownloadQueue(urls);
           
        }

        private bool downloadComplete = false;

      
        private async void FireDownloadQueue(string[] urls )
        {
            foreach (var url in urls)
            {
                await Task.Run(() => startDownload(url));
            }
        }

        string nameFile = "";

        private void startDownload(string url)
        {

            //Thread thread = new Thread(() =>
            //{
            Uri u = new Uri(url);
            nameFile = System.IO.Path.GetFileName(u.LocalPath);
            WebClient client = new WebClient();
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        client.DownloadFileAsync(new Uri(url), @"d:" + System.IO.Path.GetFileName(u.LocalPath));

                    // });
                    // thread.Start();
                    
                    while (!downloadComplete)
                    {
                        Application.DoEvents();
                    }

                    downloadComplete = false;

    

                }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
                this.BeginInvoke((MethodInvoker)delegate
                {
                    
                    double bytesIn = double.Parse(e.BytesReceived.ToString());
                    double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                    double percentage = bytesIn / totalBytes * 100;
                    label2.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                    label1.Text = nameFile;
                    progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
                });
            
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            
            this.BeginInvoke((MethodInvoker)delegate {
                label2.Text = "Completed";
                progressBar1.Value = 0;
                downloadComplete = true;
            });
           
        }
      
        
    }
}
