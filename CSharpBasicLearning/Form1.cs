using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 着重于委托的学习
/// </summary>
namespace CSharpBasicLearning
{
    public partial class Form1 : Form
    {
        private UserInfo userInfoCreate;

        //委托测试1
        public delegate void GreetingDelegate(string name);
        public Form1()
        {
            InitializeComponent();

            GreetingDelegate greetingDelegate1, greetingDelegate2,greetingDelegate3,greetingDelegate4,greetingDelegate5,greetingDelegate6;
            greetingDelegate1 = new GreetingDelegate(Greet1);
            greetingDelegate2 = new GreetingDelegate(Greet2);
            greetingDelegate1 = Greet1;
            greetingDelegate2 = Greet2;
            greetingDelegate3 = Greet1;
            greetingDelegate3 += Greet2;
            greetingDelegate3 -= Greet1;
            greetingDelegate4 = delegate (string name) { Console.WriteLine("another name is " + name); };
            greetingDelegate5 = (string name) => { Console.WriteLine("newest method is {0}", name); };
            greetingDelegate6 = name => { Console.WriteLine("lambda method is {0}", name); };
            
            greetingDelegate1("11111");
            greetingDelegate2("22222");
            greetingDelegate3("33333");
            greetingDelegate4("44444");
            greetingDelegate5("55555");
            greetingDelegate6("66666");
        }

        private void Greet1(string name)
        {
            Console.WriteLine("This is 1 " + name);
        }

        private void Greet2(string name)
        {
            Console.WriteLine("This is 2 " + name);
        }

        private void GreetPeople(string name, GreetingDelegate greetingDelegate)
        {
            Console.WriteLine(name + " O K");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateForm createForm = new CreateForm();
            createForm.onUserInfoCreate = doSomething;
            //Action是void返回值的委托
            createForm.onUserInfoCreate = new Action<UserInfo>(doSomething);
            createForm.onUserInfoCreate = (userInfo) => { userInfoCreate = userInfo; };



            createForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayForm displayForm = new DisplayForm(userInfoCreate);
            displayForm.Show();
        }
        public void doSomething(UserInfo userInfo)
        {
            userInfoCreate = userInfo;
        }
    }
}
