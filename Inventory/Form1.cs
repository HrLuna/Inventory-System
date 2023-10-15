using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory
{
    public partial class frmAddProduct : Form
    {
        private string _ProductName;
        private string _Category;
        private string _MfgDate;
        private string _ExpDate;
        private string _Description;
        private int _Quantity;
        private double _SellPrice;

        private BindingSource showProductList;
        public frmAddProduct()
        {
            InitializeComponent();
            showProductList = new BindingSource();
        }
        public class NumberFormatException : Exception
        {
            public NumberFormatException(string varName) : base(varName) { }
        }

        public class StringFormatException : Exception
        {
            public StringFormatException(string varName) : base(varName) { }
        }

        public class CurrencyFormatException : Exception
        {
            public CurrencyFormatException(string varName) : base(varName) { }
        }

        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            string[] ListOfProductCategory = new string[]
                {
                    "Beverages", "Bread/bakery", "Canned/ Jarred Goods",
                    "Dairy", "Frozen Goods", "Meat", "Personal Care", "Others"
                };
            foreach (string Category in ListOfProductCategory) 
            {
                cbCategory.Items.Add(Category);
            }
        }
        public string Product_Name(string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                _ProductName = name;
            }
            return name;
        }
        public int Quantity(string qty)
        {
            if (!Regex.IsMatch(qty, @"^[0-9]"))
            {
                _Quantity = Convert.ToInt32(qty);
            }
            return _Quantity;
        }
        public double SellingPrice(string price)
        {
            if (!Regex.IsMatch(price.ToString(), @"^(\d*\.)?\d+$"))
            {
                _SellPrice = Convert.ToDouble(price);
            }
            return _SellPrice;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                _ProductName = txtProductName.Text;
                _Category = cbCategory.Text;
                _MfgDate = dtPickerMfgDate.Value.ToString("yyyy-MM-dd");
                _ExpDate = dtPickerExpDate.Value.ToString("yyyy-MM-dd");
                _Description = richtxtDescription.Text;

                if (Regex.IsMatch(_ProductName, @"\d"))
                {
                    throw new StringFormatException("Product Name");
                }
                if (!int.TryParse(txtQuantity.Text, out _Quantity))
                {
                    throw new NumberFormatException("Quantity");
                }

                if (!double.TryParse(txtSellPrice.Text, out _SellPrice))
                {
                    throw new CurrencyFormatException("Sell Price");
                }
                showProductList.Add(new ProductClass(_ProductName, _Category, _MfgDate,
                _ExpDate, _SellPrice, _Quantity, _Description));
                gridViewProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridViewProductList.DataSource = showProductList;
            }
            catch (NumberFormatException ex)
            {
                MessageBox.Show($"Invalid input for {ex.Message}. Please enter a valid number.");
            }
            catch (CurrencyFormatException ex)
            {
                MessageBox.Show($"Invalid input for {ex.Message}. Please enter a valid currency value.");
            }
            catch (StringFormatException ex)
            {
                MessageBox.Show($"Invalid input for {ex.Message}. Please enter a valid string.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
