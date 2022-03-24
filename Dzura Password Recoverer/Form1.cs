using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace Dzura_Password_Recoverer
{
    public partial class Dzura : Form
    {
        string rich1;
        string rich2;

        StringBuilder csvcontent = new StringBuilder();
        string[] userNames = new string[300];
        string[] passcodes = new string[300];
        public Dzura()
        {
            InitializeComponent();
            folder_handling();
            check_file_csv();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
        }
        public int populate()
        {

           
           

            rich2 = rich1.Replace("All User Profile", "");

            rich2 = rich2.Remove(0, 139);
            string[] work = rich2.Split(':');



            for (int i = 0; i < work.Length; i++)

            {
                dataGridView1.Rows.Add(work[i]);

                string temp = work[i];
                temp = string.Join("", temp.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                userNames[i] = temp;

            }

            string toDisplay = string.Join(Environment.NewLine, userNames);
            MessageBox.Show(toDisplay);

            return work.Length;
        }

        public void folder_handling() {
       
                string path_real = Environment.CurrentDirectory + "//nukes";

                if (!Directory.Exists(path_real))
                {
                    Directory.CreateDirectory(path_real);
                }

        }
        public void check_file_csv()
        {
            if (File.Exists(Environment.CurrentDirectory + "//nukes//"+ Environment.UserName + ".csv"))
            {
                File.Delete(Environment.CurrentDirectory + "//nukes//" + Environment.UserName + ".csv");

                MessageBox.Show("File " + Environment.UserName + ".csv is deleted");
            }
        }


        public void networks()
        {
            Process pass = new Process();
            pass.StartInfo.CreateNoWindow = true;
            pass.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pass.StartInfo.FileName = "netsh";
            pass.StartInfo.UseShellExecute = false;
            pass.StartInfo.Arguments = "wlan show profiles";
            pass.StartInfo.RedirectStandardOutput = true;
            pass.Start();
            rich1 = pass.StandardOutput.ReadToEnd();

        }

        public void passwords()
        {
            int bound = populate();

            Process pass = new Process();
            Process filter = new Process();
            pass.StartInfo.CreateNoWindow = true;
            filter.StartInfo.CreateNoWindow = true;
            pass.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            filter.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pass.StartInfo.FileName = "netsh";
            filter.StartInfo.FileName = "fi";
            pass.StartInfo.UseShellExecute = false;

            for (int i = 1; i < bound; i++)
            {
                string rep = dataGridView1.Rows[i].Cells[0].Value.ToString();
                rep = rep.Trim();
                pass.StartInfo.Arguments = "wlan show profile \"" + rep + "\" key=clear";

                pass.StartInfo.RedirectStandardOutput = true;
                pass.Start();
                string temp = pass.StandardOutput.ReadToEnd();

                int ss = temp.IndexOf("Key Content");
                temp = temp.Replace("Key Content            :", "");
                int absent = temp.IndexOf("Security key           : Absent");
                int limit = temp.IndexOf("Cost");
                limit = limit - ss;
                if (ss > 0)
                {
                    dataGridView1.Rows[i].Cells[1].Value = temp.Substring(ss, limit - 4).Trim();
                    passcodes[i] = temp.Substring(ss, limit - 4).Trim();
                }

                else
                {
                    dataGridView1.Rows[i].Cells[1].Value = "None";
                    passcodes[i] = "None";
                }
                if (absent > 0)
                {
                    dataGridView1.Rows[i].Cells[1].Value = "None";
                    passcodes[i] = "None";
                }
               
            }
            string toDisplay = string.Join(Environment.NewLine, passcodes);
            MessageBox.Show(toDisplay);

            for (int i = 1; i < userNames.Length; i++)
            {
                csvcontent.AppendLine(""+userNames[i]+","+passcodes[i]+"");

            }

            string csvpath = Path.Combine(Environment.CurrentDirectory, "nukes//"+Environment.UserName+".csv");
            File.AppendAllText(csvpath,csvcontent.ToString());




        }

        public void cancel()
        {
            if (dataGridView1.Rows[0].Cells[0].Value.ToString().Contains("----"))
            {
                dataGridView1.Rows.RemoveAt(0);
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "English")
            {
                dataGridView1.Columns[0].HeaderCell.Value = "NAME";
                dataGridView1.Columns[1].HeaderCell.Value = "Password";
                label4.Text = "Help me help you:";
                button1.Text = "Donate";

            }
            else if (comboBox1.Text == "Shona")
            {
                dataGridView1.Columns[0].HeaderCell.Value = "ZITA";
                dataGridView1.Columns[1].HeaderCell.Value = "Svumbunuro";
                label4.Text = "Batsira Mugadziri:";
                button1.Text = "Yamura";
            }
            else if (comboBox1.Text == "Jecha")
            {
                dataGridView1.Columns[0].HeaderCell.Value = "NGODA";
                dataGridView1.Columns[1].HeaderCell.Value = "Gunanzi";
                label4.Text = "Ndijegewo wangu:";
                button1.Text = "Batsira";
            }
        }

        private void Dzura_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "";

            string business = "pmanstylz@gmail.com";  // your paypal email
            string description = "Donation";            // '%20' represents a space. remember HTML!
            string country = "ZW";                  // AU, US, etc.
            string currency = "USD";                 // AUD, USD, etc.

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            System.Diagnostics.Process.Start(url);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            help hp = new help();
            hp.ShowDialog();
        }
    }
}
