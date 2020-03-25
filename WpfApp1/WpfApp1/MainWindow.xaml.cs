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
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string firstval = "", secondval = "";
        //string path = "XML_dialy";
        double amount;
        Valute val1 = new Valute();
        Valute val2 = new Valute();
        List<Valute> valutes = new List<Valute>();
        XmlDocument xDoc = new XmlDocument();
        //private bool file_opened; //Был ли открыт файл
        private string file_path = ""; //Путь к файлу
        private string filter = "ASP file(*.asp)|*.asp|XML file(*.xml)|*.xml|All(*.*)|*";
        //public string[] config = File.ReadAllLines("config.txt");
        //private string valcurs_name = "";
        //private string valcurs_date = "";

        public MainWindow()
        {
            InitializeComponent();
            //Load();
           
        }

        private void Load_Currency(string path)
        {
            try
            {
                xDoc.Load(@path);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Чет инета нет ( потому что {ex.Message.ToString().ToLower()})", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            XmlElement xRoot = xDoc.DocumentElement;
            
            log_info.Inlines.Add($"{xRoot.Attributes[1].Value}\n");
            log_info.Inlines.Add($"{xRoot.Attributes[0].Value}\n");
            log_info.Inlines.Add("-----------------------------\n");
            foreach (XmlNode xnode in xRoot)
            {
                Valute valute = new Valute();
                valute.ID = xnode.Attributes[0].Value;
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
            valutes.Add(new Valute()
            {
                CharCode = "RUR",
                Nominal = 1,
                Name = "Российский рубль",
                Value = 1,
                NumCode = "810"
            });

            foreach (Valute valute in valutes)
            {
                first_curname.Items.Add($"{valute.Name}");
                second_curname.Items.Add($"{valute.Name}");
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
        private void File_Open(object sender, RoutedEventArgs e)
        {   //Диалоговое окно открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = filter };
            if (openFileDialog.ShowDialog() == true) {
                
                Load_Currency(openFileDialog.FileName);
                

            }
            //file_opened = true;
            file_path = openFileDialog.FileName;
            this.Title = $"{file_path} - {this.Title}";
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Currency_Info(object sender, RoutedEventArgs e)
        {
           
            if ((sender as MenuItem).Header.ToString() == "First currency")
            {
              
                /*log_info.Text += $"ID: {val1.ID}\n" +
                                 $"Number: {val1.NumCode}\n" +
                                 $"Code: {val1.CharCode}\n" +
                                 $"Name: {val1.Name}\n" +
                                 $"Nominal: {val1.Nominal}\n" +
                                 $"Value: {val1.Value}\n" +
                                 $"-----------------------------\n";*/
                log_info.Inlines.Add($"ID: {val1.ID}\n");
                log_info.Inlines.Add($"Number: {val1.NumCode}\n");
                log_info.Inlines.Add($"Code: {val1.CharCode}\n");
                log_info.Inlines.Add($"Name: {val1.Name}\n");
                log_info.Inlines.Add($"Nominal: {val1.Nominal}\n");
                log_info.Inlines.Add($"Value: {val1.Value}\n");
                log_info.Inlines.Add($"-----------------------------\n");
            }
            if ((sender as MenuItem).Header.ToString() == "Second currency")
            {
                
                log_info.Inlines.Add($"ID: {val2.ID}\n");
                log_info.Inlines.Add($"Number: {val2.NumCode}\n");
                log_info.Inlines.Add($"Code: {val2.CharCode}\n");
                log_info.Inlines.Add($"Name: {val2.Name}\n");
                log_info.Inlines.Add($"Nominal: {val2.Nominal}\n");
                log_info.Inlines.Add($"Value: {val2.Value}\n");
                log_info.Inlines.Add($"-----------------------------\n");
            }
        }
        
            private void Help_About(object sender, RoutedEventArgs e)
            {
                MessageBox.Show("[$] CurrencyConvector\n\nVersion: 2.0\n_________________________________________\n(c) Artyom Lazarev | https://github/norecord", "About");
            }
        
        private void Copy(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).Header.ToString() == "Result")
            {
             Clipboard.SetText(second_cur.Content.ToString());
            }
            if ((sender as MenuItem).Header.ToString() == "Log")
            {
                Clipboard.SetText(log_info.Text);
            }
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
           log_info.Inlines.Clear();
        }
        private void Load_button(object sender, RoutedEventArgs e)
        {
            file_path = $"http://www.cbr.ru/scripts/XML_daily.asp?{date_req.Text}";
            Load_Currency(file_path);
            this.Title = $"{file_path} - CurrencyConvector";
        }

        class Valute
        {
            public string NumCode { get; set; }
            
            public string ID { get; set; }
            public string CharCode { get; set; }
            public uint Nominal { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }


       
    }
}
