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
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace StorytelApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StorytelDbClassDataContext db = new StorytelDbClassDataContext();

        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Handled)
            {
                AraLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 85, 0));
            }
            else
            {
                AraLbl.Foreground = new SolidColorBrush(Color.FromArgb((byte)86.7, 0, 0, 0));

            }
        }

        private void AraLbl_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //AraLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 85, 0));
        }

        private void AraLbl_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //AraLbl.Foreground = new SolidColorBrush(Color.FromArgb((byte)86.7, 0, 0, 0));
        }
    }
}
