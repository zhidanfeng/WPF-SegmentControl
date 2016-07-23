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
    /// SegmentControl.xaml 的交互逻辑
    /// </summary>
    public partial class SegmentControl : UserControl
    {
        #region 依赖属性

        #region ItemsSource数据源
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource"
            , typeof(System.Collections.IEnumerable), typeof(SegmentControl));
        /// <summary>
        /// 水印
        /// </summary>
        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region DisplayMemberPath
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath"
            , typeof(string), typeof(SegmentControl));
        /// <summary>
        /// 显示的
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        #endregion

        public SegmentControl()
        {
            InitializeComponent();

            this.Loaded += SegmentControl_Loaded;
        }

        private void SegmentControl_Loaded(object sender, RoutedEventArgs e)
        {
            var borderList = this.FindVisualChildren<Border>(this.listbox, "lineBorder").ToList();
            for(int i = 0; i < borderList.Count; i++)
            {
                if(i == listbox.ItemContainerGenerator.Items.Count - 1)
                {
                    var border = borderList[i];
                    border.Visibility = Visibility.Collapsed;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //注意赋值的地方，需现在OnApplyTemplate赋予ItemsSource，然后在Loaded中查找元素时才能找得到
            this.listbox.ItemsSource = this.ItemsSource;
            this.listbox.DisplayMemberPath = this.DisplayMemberPath;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.listbox.SelectedIndex;
            var listboxitem = this.listbox.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement;
            var border = this.FindChild<Border>(listboxitem, "border");
            
            if(border != null)
            {
                if (index == 0)
                {
                    border.CornerRadius = new CornerRadius(5, 0, 0, 5);
                }
                else if (index == listbox.ItemContainerGenerator.Items.Count - 1)
                {
                    border.CornerRadius = new CornerRadius(0, 5, 5, 0);
                }
                else
                {
                    border.CornerRadius = new CornerRadius(0, 0, 0, 0);
                }
            }
        }

        #region 元素查找
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj, string childName) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    var frameworkElement = child as FrameworkElement;
                    if (child != null && child is T && frameworkElement.Name.Equals(childName))
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child, childName))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                T childType = child as T;
                if (childType == null)
                {
                    // 住下查要找的元素
                    foundChild = FindChild<T>(child, childName);

                    // 如果找不到就反回
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // 看名字是不是一样
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        //如果名字一样返回
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // 找到相应的元素了就返回 
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        #endregion
    }
}
