using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace MarkAddIn
{
    public partial class FormHost : Form
    {
        public FormHost()
        {
            InitializeComponent();
        }

        private void FormHost_Load(object sender, EventArgs e)
        {
         

            Show();

            /*
             {Dock = DockStyle.Fill,
             Child = new MainWindow()
            };
            
             this.Controls.Add(host);
            this.Text = "Mark"; 
            this.Size = new Size (800, 600);*/


        }
    }
}
