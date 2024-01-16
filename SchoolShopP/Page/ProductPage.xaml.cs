using SchoolShopP.Database;
using SchoolShopP.DB;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage 
    {
        List<Product> products = new List<Product>();
        public ProductPage()
        {
            InitializeComponent();
            products = DataBase.entities.Product.ToList();
            ListViewProducts.ItemsSource = products;
            ManufComboBox.Items.Add(new Manufacturer() { Name = "Все производители" });
            foreach (var i in DataBase.entities.Manufacturer.ToList())
            {
                ManufComboBox.Items.Add(i);
            }
        }
        //совместная сортировка, фильтрация и поиск товаров
        private void SearhBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearhBox.Text == "")
            {
                products = DataBase.entities.Product.ToList();
                ListViewProducts.ItemsSource = products;
                ManufComboBox_SelectionChanged(null, null);
                UpButton_Click(null, null);
                DownButton_Click(null,null);
                AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
            }
            else
            {
                products = products.Where(u => u.Title.ToLower()
                .Contains(SearhBox.Text.ToLower()) || u.Cost.ToString().Contains(SearhBox.Text.ToLower()) || u.Description.ToLower().Contains(SearhBox.Text.ToLower())).ToList();
                ListViewProducts.ItemsSource = products;
                ManufComboBox_SelectionChanged(null, null);
                UpButton_Click(null, null);
                DownButton_Click(null, null);
                AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
            }
            if (ListViewProducts.HasItems == false)
            {
                MessageBox.Show("Ничего не найдено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
                SearhBox.Text = string.Empty ;
            }
            else
            {
                ManufComboBox_SelectionChanged(null, null);
                UpButton_Click(null, null);
                DownButton_Click(null, null);
                AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
            }
            AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
        }
        //загрузка данных
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            products = DataBase.entities.Product.ToList();
            ListViewProducts.ItemsSource = products;
        }
        //загрузка допольнительного поля "Все производители" для ManufactureComboBox
        private void ManufComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Manufacturer manufacturer = (Manufacturer)ManufComboBox.SelectedItem;
                if (manufacturer.Name != "Все производители")
                {
                    ListViewProducts.ItemsSource = products.Where(c => c.Manufacturer.ID == manufacturer.ID).ToList();
                }
                else
                {
                    ListViewProducts.ItemsSource = products;
                }
                AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
            }
            catch { MessageBox.Show("Что-то пошло не так"); }
        }
        //кнопка сортировка товаров по возрастанию
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            products = products.OrderBy(c => c.Cost).ToList();
            ListViewProducts.ItemsSource = products;
            ManufComboBox_SelectionChanged(null, null);
        }
        //кнопка сортировка товаров по убыванию
        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            products = products.OrderByDescending(c => c.Cost).ToList();
            ListViewProducts.ItemsSource = products;
            ManufComboBox_SelectionChanged(null, null);
        }

        Product curentproduct;
        //переход при двойном нажатии на страницу редактирования товара, проверка на "активность"
        private void ProductBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
             curentproduct = (Product)ListViewProducts.SelectedItem;
            if (e.ClickCount == 2)
            {
                if (curentproduct.IsActive == false)
                {
                    MessageBox.Show("Данный товар не активен.");
                }
                else
                {
                    NavigationService.Navigate(new AddEditePage(curentproduct));
                }
            }
        }
        //свойство по умолчанию для ManufacturecomboBox 
        private void CancelFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ManufComboBox.SelectedIndex = 0;
        }
        //кнопка для перехода добавления товара
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page.AddEditePage());
        }
        //кнопка для удаления товара
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            if (ListViewProducts.SelectedItem == null)
                {
                    MessageBox.Show("Вы не выбрали товар", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var it = ListViewProducts.SelectedItem as Product;
                        if (DataBase.entities.ProductSale.Where(x=>x.ProductID == it.ID).ToList().Count==0)
                        {
                            if (DataBase.entities.AttachedProduct.Where(x => x.MainProductID == it.ID).ToList().Count == 0)
                            {

                            DataBase.entities.DeleteProduct(it.ID);
                                MessageBox.Show("Удаление прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Нельзя удалить, есть допольнительные товары", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Нельзя удалить, товар есть в заказах", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    ListViewProducts.ItemsSource = DataBase.entities.Product.ToList();
                    AmountTextBlock.Text = $"{ListViewProducts.Items.Count} из {DataBase.entities.Product.ToList().Count}";
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Ошибка", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        //кнопка для перехода на страницу с редактированием товара
        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ListViewProducts.SelectedItem == null)
                {
                    MessageBox.Show("Вы не выбрали товар", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var it = ListViewProducts.SelectedItem as Product;
                    NavigationService.Navigate(new Page.AddEditePage(it));
                }
            }
            catch
            {
                MessageBox.Show("Ошибка", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //кнопка перехода на страницу с заказами
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewProducts.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали товар", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var it = ListViewProducts.SelectedItem as Product;
                
                    NavigationService.Navigate(new Page.OrderPage(it));
                
            }
        }

        private void OutApplication_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите выйти", "Системное уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
