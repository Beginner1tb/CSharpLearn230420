﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_ControlAdd
{
    /// <summary>
    /// UserControl_Test1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_Test1 : UserControl
    {
        public UserControl_Test1()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Btn1.Content = _Text;
        }

        private string text1;
        public string _Text

        {
            get { return text1; }
            set { text1 = value; }
        }
    }


}
