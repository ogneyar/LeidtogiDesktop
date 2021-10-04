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

namespace LeidtogiDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Colors.Orange);

            Button myButton = new Button();
            myButton.Width = 100;
            myButton.Height = 30;
            myButton.Content = "Тест"; 
            myButton.Background = new SolidColorBrush(Colors.Gray);

            stackPanelForAddButton.Children.Add(myButton);

            // Image myImage = new Image(); 
            // myImage.Source = new BitmapImage(new Uri("src/img/favicon.ico", UriKind.Relative));

            // stackPanelForAddImage.Children.Add(myImage);

        }
       
    }

    public class Phone
    {
        public string Name { get; set; }
        public int Price { get; set; }
        
        public override string ToString()
        {
            return $"Смарт {this.Name} цена: {this.Price}";
        }
    }


}
