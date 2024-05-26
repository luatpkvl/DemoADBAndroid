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

namespace WhatsappAccount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadToManagerWindow();
        }
        /// <summary>
        /// Load ra màn hình chính
        /// created by: ltluat 03.06.2023
        /// </summary>
        private void LoadToManagerWindow()
        {
            ManagerWindow window= new ManagerWindow();  
            window.Show();
            this.Close();
        }
    }
}
