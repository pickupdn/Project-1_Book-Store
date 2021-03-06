﻿using System;
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
using System.Windows.Shapes;
using System.Xml;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for CustomersMenu.xaml
    /// </summary>
    public partial class CustomersMenu : Window
    {
        //List<CustomerList> customerList;

        private List<List<string>> searchResult;
        private string tableName = "Customers";
        private string customerId;
        public CustomersMenu()
        {
            InitializeComponent();
        }
        private void CustomersMenu_Loaded(object sender, RoutedEventArgs e)
        {
            searchList.Items.Add("ทั้งหมด");
            searchList.Items.Add("รหัสสมาชิก");
            searchList.Items.Add("ชื่อลูกค้า");
            searchList.Items.Add("Email");
            searchList.Text = "ทั้งหมด";
        }
        public class Table
        {
            public string customerId { get; set; }

            public string customerName { get; set; }

            public string email { get; set; }

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            searchResult = DataAccess.SearchItemExact(tableName, "Customer_Id", customerIdTextBox.Text);
            if (!searchResult.Any())
            {
                DataAccess.AddData(tableName, customerIdTextBox.Text, customerNameTextBox.Text, addressTextBox.Text, emailTextBox.Text, "");
            }
            else MessageBox.Show("Customer Id : " + customerIdTextBox.Text + " already existed.");

            customerIdTextBox.Text = "";
            customerNameTextBox.Text = "";
            addressTextBox.Text = "";
            emailTextBox.Text = "";
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchItem = searchTextBox.Text;
            string searchField = searchList.Text;

            if (searchField == "รหัสสมาชิก")
            {
                searchField = "Customer_Id";
            }
            else if (searchField == "ชื่อลูกค้า")
            {
                searchField = "Customer_Name";
            }
            else if (searchField == "ทั้งหมด")
            {
                searchField = "All";
            }
            if (searchField == "All")
            {
                //MessageBox.Show("Go all search.");
                searchResult = DataAccess.SearchItem(tableName, searchField, searchItem);
                dataSearchShow(searchResult);
            }
            else if (searchField == "Customer_Id" || searchField == "Customer_Name" || searchField == "Email")
            {
                searchResult = DataAccess.SearchItem(tableName, searchField, searchItem);
                dataSearchShow(searchResult);
            }
            else { MessageBox.Show("Search Error."); }
        }

        public void dataSearchShow(List<List<string>> searchResult)
        {
            List<List<string>> dataFound = new List<List<string>>();
            int i = 0;
            foreach (List<string> searchItem in searchResult)
            {
                dataFound.Add(searchItem);
                i++;
            }
            if (dataFound.Count == 0)
            {
                MessageBox.Show("Data Not Found.");
            }
            else
            {
                List<DataList> customerList = new List<DataList>(); 
                List<Table> items = new List<Table>();
                int numberOfList = dataFound.Count();
                for (int j = 0; j < numberOfList; j++)
                {
                    customerList.Add(new DataList(dataFound[j][0], dataFound[j][1], dataFound[j][2], dataFound[j][3]));
                }
                customersListView.ItemsSource = customerList;
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            searchResult = DataAccess.SearchItem(tableName, "Customer_Id", customerIdTextBox.Text);
            if (!searchResult.Any())
            {
                MessageBox.Show("Unable to update data.\nCustomer ID : " + customerIdTextBox.Text + " not existed.");
            }
            else
            {
                if (Dialog.Confirm("Edit"))
                {
                    DataAccess.UpdateData(tableName, customerIdTextBox.Text, customerNameTextBox.Text, addressTextBox.Text, emailTextBox.Text, "");
                    MessageBox.Show("Update succesfully");
                }
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Dialog.Confirm("Delete"))
            {
                DataAccess.DeleteData(tableName, customerIdTextBox.Text);
                MessageBox.Show("Delete succesfully");
            }
        }

        private void customersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataList selectedCustomer = (DataList)customersListView.SelectedItem;
            if (selectedCustomer != null)
            {
                this.customerId = selectedCustomer.CustomerId;
                customerIdTextBox.Text = selectedCustomer.CustomerId;
                customerNameTextBox.Text = selectedCustomer.CustomerName;
                addressTextBox.Text = selectedCustomer.Address;
                emailTextBox.Text = selectedCustomer.Email;
            }

        }

        private void customerIdTextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            customerIdTextBox.Text = customerIdTextBox.Text.PadLeft(6, '0');
        } 
    }

}
