using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiaDetector.SubForm
{
    public partial class SmallLog : Form
    {
        private static SmallLog instance = new SmallLog();

        public static SmallLog Instance
        {
            get
            {
                return instance;
            }
        }

        string path = System.IO.Path.Combine(@"C:\KSM\DiaDetector", "SmallLog.txt");
        string[] textArr;

        private SmallLog()
        {
            InitializeComponent();
        }

        private void SmallLog_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.CreateText(path).Dispose();
            }

            textArr = System.IO.File.ReadAllLines(path);

            ListViewItem item;

            for (int i = 0; i < textArr.Length; i++)
            {
                item = new ListViewItem();

                item.Text = (i + 1).ToString();
                item.SubItems.Add(textArr[i]);

                listView1.Items.Add(item);
            }
        }

        public void LogReport(string log)
        {
            string timeLog = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff") + "     =>     " + log;

            ListViewItem item = new ListViewItem();

            item.Text = (listView1.Items.Count + 1).ToString();
            item.SubItems.Add(timeLog);

            listView1.Items.Add(item);

            LogSave();
        }

        void LogSave()
        {
            List<String> data = new List<String>();
            string text = "";

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                data.Add(listView1.Items[i].SubItems[1].Text);
            }

            foreach (string str in data)
            {
                text += str + "\r\n";
            }

            System.IO.File.AppendAllText(path, text);
        }

        private void buttonLogSave_Click(object sender, EventArgs e)
        {
            LogSave();
        }

        private void buttonLogDelete_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(path, "");

            listView1.Items.Clear();
        }
    }
}
