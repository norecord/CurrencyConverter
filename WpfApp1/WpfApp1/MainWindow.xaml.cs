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
using System.Xml;
using System.IO;
using System.Net;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string firstval = "", secondval = "";
        string path = "XML_dialy";
        double amount;
        Valute val1 = new Valute();
        Valute val2 = new Valute();
        List<Valute> valutes = new List<Valute>();
        XmlDocument xDoc = new XmlDocument();
        //public string[] config = File.ReadAllLines("config.txt");

        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                xDoc.Load(@"http://www.cbr.ru/scripts/XML_daily.asp");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Чет инета нет ( потому что {ex.Message.ToString().ToLower()})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                Valute valute = new Valute();
                foreach (XmlNode chlnode in xnode.ChildNodes)
                {
                    if (chlnode.Name == "CharCode")
                    {
                        valute.CharCode = chlnode.InnerText;
                    }
                    if (chlnode.Name == "Nominal")
                    {
                        valute.Nominal = Convert.ToUInt32(chlnode.InnerText);
                    }
                    if (chlnode.Name == "Name")
                    {
                        valute.Name = chlnode.InnerText;
                    }
                    if (chlnode.Name == "Value")
                    {
                        valute.Value = Convert.ToDouble(chlnode.InnerText);
                    }
                    if (chlnode.Name == "NumCode")
                    {
                        valute.NumCode = chlnode.InnerText;
                    }
                }
                valutes.Add(valute);
            }
            foreach (Valute valute in valutes)
            {
                first_curname.Items.Add(valute.Name);
                second_curname.Items.Add(valute.Name);
            }
        }
       
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = (string)comboBox.SelectedItem;
            firstval = selectedItem;

        }

        private void ComboBox_Selected_2(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = (string)comboBox.SelectedItem;
            secondval = selectedItem;

        }
        private void Translate(object sender, RoutedEventArgs e)
        {          
            val1 = valutes.FirstOrDefault(x => x.Name == firstval);           
            val2 = valutes.FirstOrDefault(x => x.Name == secondval);
            try
            {
                amount = Convert.ToDouble(first_cur.Text);
                double amount2 = val1.Value / val1.Nominal;
                double amount3 = val2.Value / val2.Nominal;
                double final_value = amount * (amount2 / amount3);
                second_cur.Content = final_value.ToString();            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message.ToString()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        class Valute
        {
            public string NumCode { get; set; }
            public string CharCode { get; set; }
            public uint Nominal { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }
    }
}
