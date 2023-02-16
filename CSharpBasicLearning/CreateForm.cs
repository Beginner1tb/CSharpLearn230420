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
    public partial class CreateForm : Form
    {
        //委托一般不初始化
        public Action<UserInfo> onUserInfoCreate;
        public delegate void onUserInfoCreate_1(UserInfo userInfo);
        
        public CreateForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserInfo userInfo = new UserInfo();
            
            userInfo.num = textBox1.Text;
            userInfo.name = textBox2.Text;
            //?表示判断变量是否存在
            //onUserInfoCreate?.Invoke(userInfo);
            if (onUserInfoCreate != null)
            {
                onUserInfoCreate.Invoke(userInfo);
            }
            //没有实例化不分配内存空间，无法调用
            onUserInfoCreate_1 onUserInfoCreate1 = new onUserInfoCreate_1(doSomething);
            if (onUserInfoCreate1 != null)
            {
                onUserInfoCreate1.Invoke(userInfo);
            }
            this.Close();
        }

        private void doSomething(UserInfo userInfo)
        {
            
        }
    }
}
