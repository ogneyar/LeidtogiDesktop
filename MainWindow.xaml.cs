using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using System.Reflection;



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

            var root = Directory.GetCurrentDirectory();
            var dotenv = System.IO.Path.Combine(root, ".env");
            LoadEnv(dotenv);

            string url = Environment.GetEnvironmentVariable("URL_API");

            GetProducts(url);

            this.SizeChanged += OnWindowSizeChanged;
            
            Exit.Click += ExitClick;

            this.Background = new SolidColorBrush(Colors.Orange);

            Header.Background = new SolidColorBrush(Colors.Gray);

            Button myButton = new Button();
            myButton.Width = 100;
            myButton.Height = 30;
            myButton.Content = "Пуск";
            myButton.Margin = new Thickness(0, 5, 5, 0);
            myButton.Background = new SolidColorBrush(Colors.White);
            myButton.Foreground = new SolidColorBrush(Colors.Red);
            myButton.HorizontalAlignment = HorizontalAlignment.Right;
            myButton.Cursor = Cursors.Hand;
            myButton.Click += ClickButtonToRequest;
            // myButton.Click += ClickButtonToTest;
            // Добавление в футер кнопки 'Пуск'
            Footer.Children.Add(myButton);

            // string filePath;
            // Image myImage;
            // filePath = System.AppDomain.CurrentDomain.BaseDirectory + "src\\img\\logo.bmp";
            // if (File.Exists(filePath)) {
            //     myImage = new Image();
            //     myImage.Source = new BitmapImage(new Uri(filePath));
            //     myImage.Width = 90;
            //     myImage.HorizontalAlignment = HorizontalAlignment.Right;
            //     myImage.Margin = new Thickness(5);
            //     // Добавление в тело картинки
            //     Body.Children.Add(myImage);
            // }
            // filePath = url + "milwaukee/4933471080/big/e13e52e0-dee0-4f7e-aa3c-ae0c5718090c.jpg";
            // myImage = new Image { 
            //     Source = new BitmapImage(new Uri(filePath)), 
            //     Width = 150 
            // };
            // // Добавление в тело картинки
            // Body.Children.Add(myImage);

        }

        public static void LoadEnv(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public async void GetProducts(string url, int limit = 9)
        {
            try {
                if (limit > 9) limit = 9;
                WebRequest request = WebRequest.Create(url + "api/product?limit=" + limit.ToString());
                // request.Headers.Add("Authorization: Bearer " + token);
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string productJSON = reader.ReadToEnd();
                            var options = new JsonSerializerOptions { IgnoreNullValues = true };
                            // Product objectProduct = JsonSerializer.Deserialize<Product>(productJSON, options);
                            ProductLimit objectProductLimit = JsonSerializer.Deserialize<ProductLimit>(productJSON, options);
                            Product[] objectProducts = objectProductLimit.rows;
                            int i = 1;
                            foreach(Product objectProduct in objectProducts) {
                                // if (objectProduct.id == 70) MessageBox.Show(objectProduct.name);

                                string imgJSON = objectProduct.img;
                                Img[] objectImg = JsonSerializer.Deserialize<Img[]>(imgJSON, options);
                                string filePath = url + objectImg[0].big;
                                Image myImage = new Image { 
                                    Source = new BitmapImage(new Uri(filePath)), 
                                    Width = 142,
                                    Margin = new Thickness(10, 10, 10, 10)
                                };
                                // Добавление в тело картинки
                                if (i == 1) Body1.Children.Add(myImage);
                                if (i == 2) Body2.Children.Add(myImage);
                                if (i == 3) Body3.Children.Add(myImage);
                                if (i == 4) Body4.Children.Add(myImage);
                                if (i == 5) Body5.Children.Add(myImage);
                                if (i == 6) Body6.Children.Add(myImage);
                                if (i == 7) Body7.Children.Add(myImage);
                                if (i == 8) Body8.Children.Add(myImage);
                                if (i == 9) Body9.Children.Add(myImage);

                                i++;

                            }

                        }
                    }
                }
            }catch {
                MessageBox.Show("Исключение!");
            }
        }

        /// <summary>
        /// Метод запускаемый по событию клика мыши по "Файл" -> "Выход"
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Было
            double prevWindowWidth = e.PreviousSize.Width;
            double prevWindowHeight = e.PreviousSize.Height;
            // Стало
            double newWindowWidth = e.NewSize.Width;
            double newWindowHeight = e.NewSize.Height;

            // if (prevWindowHeight > 0) Body.Height = Body.Height + (newWindowHeight - prevWindowHeight);
            // MessageBox.Show("Изменение размера. Было: " + prevWindowWidth.ToString() + "x" + prevWindowHeight.ToString() + ". Стало: " +  newWindowWidth.ToString() + "x" + newWindowHeight.ToString());
        }

        private void ClickButtonToTest(object sender, RoutedEventArgs e)
        {            
            if (sender.GetType() == typeof(Button))
                MessageBox.Show((sender as Button).Content.ToString());
            
            MessageBox.Show(Environment.GetEnvironmentVariable("TEST").ToString());
        }

        private async void ClickButtonToRequest(object sender, RoutedEventArgs e)
        {
            string token;
            string url = Environment.GetEnvironmentVariable("URL_API");
            try {
                // MessageBox.Show("Начало...");
                WebRequest request = WebRequest.Create(url + "api/user/login");

                request.Method = "POST"; // для отправки используется метод Post
                // request.Headers.Add("Host: api.leidtogi.site");
                // request.Headers.Add("Origin: leidtogi.site");
                // подключение cookies.
                // request.Credentials = CredentialCache.DefaultCredentials;
                
                // данные для отправки
                string email = Environment.GetEnvironmentVariable("EMAIL");
                string password = Environment.GetEnvironmentVariable("PASSWORD");
                string data = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
                // преобразуем данные в массив байтов
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                // устанавливаем тип содержимого - параметр ContentType
                request.ContentType = "application/json";
                // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
                request.ContentLength = byteArray.Length;
                        
                // записываем данные в поток запроса
                using (Stream dataStream = request.GetRequestStream()) {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                // считываем данные из ответа сервера
                using (WebResponse response = await request.GetResponseAsync()) {
                    using (Stream stream = response.GetResponseStream()) {
                        using (StreamReader reader = new StreamReader(stream)) {
                            string tokenJSON = reader.ReadToEnd();
                            Token objectToken = JsonSerializer.Deserialize<Token>(tokenJSON);
                            token = objectToken.token;
                            // MessageBox.Show(token);
                        }
                    }
                }

                request = WebRequest.Create(url + "api/user/info");
                request.Headers.Add("Authorization: Bearer " + token);
                using (WebResponse response = await request.GetResponseAsync()) {
                    using (Stream stream = response.GetResponseStream()) {
                        using (StreamReader reader = new StreamReader(stream)) {
                            string userJSON = reader.ReadToEnd();
                            // MessageBox.Show(userJSON);
                            var options = new JsonSerializerOptions { IgnoreNullValues = true };
                            User objectUser = JsonSerializer.Deserialize<User>(userJSON, options);
                            if (objectUser.role == null) MessageBox.Show("Нет такого параметра...");
                            else MessageBox.Show("Ваша роль: " + objectUser.role);
                        }
                    }
                }
                // MessageBox.Show("Запрос выполнен...");
            }catch {
                MessageBox.Show("Исключение!");
            }
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

    class Token { public string token { get; set; } }


    class User { 
        public int id { get; set; } 
        public string surname { get; set; } 
        public string name { get; set; } 
        public string patronymic { get; set; } 
        public long phone { get; set; } 
        public string email { get; set; } 
        public string address { get; set; } 
        public string password { get; set; } 
        public string role { get; set; } 
        public int isActivated { get; set; } 
        public string activationLink { get; set; } 
        public string companyName { get; set; } 
        public string INN { get; set; } 
        public string KPP { get; set; } 
        public string OGRN { get; set; } 
        public string OKVED { get; set; } 
        public string juridicalAddress { get; set; } 
        public string bank { get; set; } 
        public string BIK { get; set; } 
        public string corAccount { get; set; } 
        public string payAccount { get; set; } 
        public string post { get; set; } 
        public string createdAt { get; set; } 
        public string updatedAt { get; set; } 
    }


    class Product { 
        public int id { get; set; } 
        public string name { get; set; } 
        public string url { get; set; } 
        public int price { get; set; } 
        public float rating { get; set; } 
        public string img { get; set; } 
        public int have { get; set; } 
        public string article { get; set; } 
        public string promo { get; set; } 
        public string country { get; set; } 
        public string createdAt { get; set; } //  date
        public string updatedAt { get; set; } //  date
        public int categoryId { get; set; } 
        public int brandId { get; set; } 
        public Info[] info { get; set; } 
        public Size[] size { get; set; } 
    }

    class Info { 
        public int id { get; set; } 
        public string title { get; set; } 
        public string body { get; set; } 
        public string createdAt { get; set; } //  date
        public string updatedAt { get; set; } //  date
        public int productId { get; set; }

    }
    class Size { 
        public int id { get; set; } 
        public float weight { get; set; } 
        public float volume { get; set; } 
        public float width { get; set; } 
        public float height { get; set; } 
        public float length { get; set; } 
        public string createdAt { get; set; } //  date
        public string updatedAt { get; set; } //  date
        public int productId { get; set; }

    }

    class Img {
        public string big { get; set; }
        public string small { get; set; }
    }

    class ProductLimit {
        public int count { get; set; }
        public Product[] rows { get; set; }
    }

}
