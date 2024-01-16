using Microsoft.Win32;
using SchoolShopP.Database;
using SchoolShopP.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Логика взаимодействия для AddEditePage.xaml
    /// </summary>
    public partial class AddEditePage 
    {
        Product product;
        bool newProduct = false;
        public AddEditePage()
        {
            InitializeComponent();
            this.product = new Product();
            ManufactureComboBox.ItemsSource = DataBase.entities.Manufacturer.ToList();
            newProduct = true;
            Idtxtbox.Visibility = Visibility.Collapsed;
            IDtxtBlock.Visibility = Visibility.Collapsed;
            this.DataContext = this.product;
        }
        public AddEditePage(Product curentproduct)
        {
            InitializeComponent();
            this.product = curentproduct;
            ManufactureComboBox.ItemsSource = DataBase.entities.Manufacturer.ToList();
            this.DataContext = this.product;

            
        }
        //кнопка сохранения изменений(добавления/редактирования) товаров
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(TitleTextBox.Text) || String.IsNullOrWhiteSpace(DescriptionTextBox.Text) || String.IsNullOrWhiteSpace(CosttextBox.Text) || ManufactureComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string price = (CosttextBox.Text.Replace(".0000", ""));
                    if (decimal.TryParse(price, out decimal d))
                    {
                        if(d <= 0) MessageBox.Show("Цена не может быть отрицательной", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                                    if (newProduct)
                                    {
                                        DataBase.entities.Product.Add(product);
                                    }
                                    DataBase.entities.SaveChanges();
                                    MessageBox.Show("Операция выполнена успешно", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                    NavigationService.GoBack();
                        }
                        
                    }
                    else MessageBox.Show("Введена некорректная цена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте правильность заполненых данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //загрузка изображения товара
        private void PhotoBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image |*.png; *.jpg";
                fileDialog.ShowDialog();
                string path = fileDialog.FileName;
                product.MainImagePath = path;
                PhotoBox.Source = new BitmapImage(new Uri(path));
                product.PhotoProduct = File.ReadAllBytes(fileDialog.FileName);
            }
            catch
            {
                MessageBox.Show("Вы не выбрали изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        //кнопка для перехода на страницу назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите отменить действие?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }
        
    }
}
