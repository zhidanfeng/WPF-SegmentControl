using System;
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

namespace SegmentControlDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<string> list = new List<string>();
            list.Add("全部");
            list.Add("主任医师");
            list.Add("副主任医师");
            list.Add("住院医生");
            list.Add("其他");
            //哈哈放大师傅
            this.segmentControl.ItemsSource = list;
        }
    }
}
