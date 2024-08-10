using System;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class PopUp : Form
    {
        public PopUp()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopUp_Load(object sender, EventArgs e)
        {

        }
    }
}
