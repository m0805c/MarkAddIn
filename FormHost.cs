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
         

            ElementHost Host = new ElementHost();
            Host.Dock = DockStyle.Fill;

            //creación del elementhost



        }
    }
}
