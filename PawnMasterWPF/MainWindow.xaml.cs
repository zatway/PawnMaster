﻿using System.Windows;
using System.Windows.Controls;
using PawnMasterLibrary;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using ArslanbekovLibrary;
using ModelForPawnMaster;
using PawnMasterLibrary;

namespace PawnMasterWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    string _role;

    List<byte> imageDataList = new List<byte>();

    private Employee loggedEmployee;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ImageAdd_Click(object sender, RoutedEventArgs e)
    {
        ImageAdd();
    }

    void ImageAdd()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
       
        if (openFileDialog.ShowDialog() == true)
        {
            string selectedImagePath = openFileDialog.FileName;
            byte[] imageBytes = File.ReadAllBytes(selectedImagePath);
            foreach(byte imageByte in imageBytes)
            {
                imageDataList.Add(imageByte);
            }
        }
    }

    private void PurchaseProduct_click(object sender, RoutedEventArgs e)
    {

    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new LoginWindow();
        // loginWindow.Topmost = true;
        // loginWindow.Closed += (s, args) => { this.IsEnabled = true; };
        // this.IsEnabled = false;
        loginWindow.Show();
        Close();
    }
    
    private void AdminPanel_Click(object sender, RoutedEventArgs e)
    {
        if(_role == "A")
        {
            AdminPanelWindow adminPanelWindow = new AdminPanelWindow();
            adminPanelWindow.Show();
            Close();
        }
        else
        {
            MessageBox.Show("Недостаточно прав");
        }
    }

    public void LoggedUserAdd(Employee LoggedEmployee)
    {
        loggedEmployee = LoggedEmployee;
        NameUser.Text = loggedEmployee.EmployeeFullName;
        _role = "U";
    }

    public void LoggedAdmin()
    {
        NameUser.Text = "Админ";
        _role = "A";
    }

    private void ProductAdd_Click(object sender, RoutedEventArgs e)
    {
        string productName = NameItemTextBox.Text;
        string productDateBuy = ProductDateBuyPicker.Text;
        string productPrice = ProductPriceTextBox.Text;
        string productDescription = DescriptionTextBox.Text;
        byte[] imageData = imageDataList.ToArray();
        var newProduct = new Product()
        {
            ProductName = productName, ProductDateBuy = productDateBuy, ProductDescription = productDescription,
            ProductImageData = imageData, ProductPriceBuy = productPrice
        };
        ProductControl.AddProduct(newProduct);
    }

    private void ProductAvailabilityDataGrid_OnLoaded(object sender, RoutedEventArgs e)
    {
        List<Product> product = ProductControl.ReceivingProduct();
        ProductAvailabilityDataGrid.ItemsSource = product;
    }
}