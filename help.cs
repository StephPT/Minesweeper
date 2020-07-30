using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner_Game
{
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close(); //Closes the form that the button is located on. 
           
        }

        private void Help_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnClose;
        }
    }
}
