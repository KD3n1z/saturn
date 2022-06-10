﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saturn2
{
    public partial class ServerSetup : Form
    {
        public string serverName = "";

        public ServerSetup()
        {
            InitializeComponent();
        }

        private void ServerSetup_Load(object sender, EventArgs e)
        {
            label1.Text = serverName + " setup";
            foreach(string ver in Program.versions)
            {
                comboBox1.Items.Add(ver);
            }
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string serverDir = Path.Combine(Program.path, "servers", serverName);

            Directory.CreateDirectory(serverDir);

            SettingsFile sf = new SettingsFile();

            sf["core"] = comboBox1.Text + ".jar";
            sf["startArgs"] = "-Xmx%memM -Xms%memM -jar %core nogui";
            sf["mem"] = "1024";
            sf["java"] = "java";

            File.WriteAllText(Path.Combine(serverDir, "saturn-config.txt"), sf.ToString());

            if (!File.Exists(Path.Combine(Program.path, "server-jars", comboBox1.Text + ".jar")))
            {
                label1.Text = "downloading server.jar...";
                new Thread(DownloadJar).Start();
            }
            else
            {
                Close();
            }
        }

        void DownloadJar()
        {
            string ver = "";

            comboBox1.Invoke(new MethodInvoker(() =>
            {
                ver = comboBox1.Text;
            }));

            new WebClient().DownloadFile(GetJarUrl(ver), Path.Combine(Program.path, "server-jars", ver + ".jar"));
            Invoke(new MethodInvoker(() =>
            {
                Close();
            }));
        }

        private string GetJarUrl(string ver)
        {
            string html = new WebClient().DownloadString("https://mcversions.net/download/" + ver);
            html = html.Substring(0, html.IndexOf("server.jar") + "server.jar".Length);
            html = html.Remove(0, html.LastIndexOf("href=\"") + "href=\"".Length);
            return html;
        }
    }
}