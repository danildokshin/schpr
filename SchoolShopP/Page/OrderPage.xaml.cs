using SchoolShopP.Database;
using SchoolShopP.DB;
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

namespace SchoolShopP.Page
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage
    {
        List<OrderProduct> products = new List<OrderProduct>();
        Product _product;
        public OrderPage(Product it)
        {
            InitializeComponent();
            _product = it;
            products = DataBase.entities.OrderProduct.Where(x => x.Title == _product.Title).ToList();
            OrderListView.ItemsSource = products;
            ProductComboBox.Items.Add(new Product() { Title = "Все товары" });
            foreach (var i in DataBase.entities.Product.ToList())
            {
                ProductComboBox.Items.Add(i);
            }
        }
        //сортировка товаров по убыванию
        private void DownButton_Checked(object sender, RoutedEventArgs e)
        {
            products = products.OrderByDescending(c => c.SaleDate).ToList();
            OrderListView.ItemsSource = products;
        }
        //сортировка товаров по возрастанию
        private void UpButton_Checked(object sender, RoutedEventArgs e)
        {
            products = products.OrderBy(c => c.SaleDate).ToList();
            OrderListView.ItemsSource = products;
        }
        //кнопка для перехода на страницу назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите вернуться?", "Системное уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }
        //загрузка данных
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = products;
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Product product = (Product)ProductComboBox.SelectedItem;
                if (product.Title != "Все товары")
                {
                    OrderListView.ItemsSource = products.Where(x=>x.IDP == product.ID).ToList();
                }
                else
                {
                    OrderListView.ItemsSource = products;
                }
            }
            catch { MessageBox.Show("Что-то пошло не так"); }
        }
    }
}
