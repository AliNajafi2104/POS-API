using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class EnterWeight : Form
    {
        public EnterWeight(Image img)
        {
            InitializeComponent();
            pictureBox1.Image = img;
        }
    }
}
