using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpBasicLearning
{
    public partial class DisplayForm : Form
    {
        UserInfo userInfo1 = new UserInfo();
        public DisplayForm(UserInfo userInfo)
        {
            
            userInfo1 = userInfo;
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = userInfo1.num;
            textBox2.Text = userInfo1.name;
        }
    }
}
