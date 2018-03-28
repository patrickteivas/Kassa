﻿using System;
using System.IO;
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
using System.Text.RegularExpressions;

namespace WpfApp
{
    /*
    Looge kassarakendus kasutades WPF raamistikku.

    Müüja saab lisada ise tooteid. Tootel peab olema nimetus, hind ja kogus.
    Müüja saab lisada olemasolevaid tooteid ostukorvi, mille alusel arvutatakse välja summa, mida klient maksma peab. 
    */

    public partial class MainWindow : Window
    {
        public List<Toode> Tooted { get; set; }
        public List<Toode> Ostukorv { get; set; }
        public string TempNimetus;
        public int TempHind;

        public MainWindow()
        {
            Tooted = new List<Toode>();
            Ostukorv = new List<Toode>();

            InitializeComponent();
        }

        private void Lisa(object sender, RoutedEventArgs e)
        {
            TempNimetus = Nimetus.Text;
            TempNimetus = Nimetus.Text;
            bool result = int.TryParse(Hind.Text, out TempHind);

            if (TempNimetus == "")
            {
                MessageBox.Show("Toote nimetus pole lisatud!", "Viga");
            }
            else
            {
                if (result == false)
                {
                    MessageBox.Show("Vale hind!", "Viga");
                }
                else
                {
                    bool State = true;
                    foreach (var item in Tooted)
                    {
                        if(item.Nimetus == TempNimetus) State = false;
                    }
                    if (State == true)
                    {
                        Tooted.Add(new Toode() { Nimetus = TempNimetus, Hind = TempHind, Kogus = 0 });
                        Nimetus.Text = Nimetus.Name;
                        Hind.Text = Hind.Name;
                    }
                    else if (State == false)
                    {
                        MessageBox.Show("Niisugune toode on juba olemas!", "Viga");
                    }
                }
            }
            TootedList.ItemsSource = null;
            TootedList.ItemsSource = Tooted;
        }

        private void CartAdd(object sender, RoutedEventArgs e)
        {
            if (TootedList.SelectedIndex > -1)
            {
                var OnOlemas = Ostukorv.Where(p => String.Equals(p.Nimetus, Tooted[TootedList.SelectedIndex].Nimetus, StringComparison.CurrentCulture));

                if (OnOlemas.Any())
                {
                    foreach (var item in OnOlemas) item.Kogus++;
                }
                else
                {
                    Ostukorv.Add(Tooted[TootedList.SelectedIndex]);
                    Ostukorv[Ostukorv.Count - 1].Kogus++;
                }

                //string ValitudToode = (TootedList.SelectedItem as Toode).Nimetus;
                ////Ostukorv = (from item in Ostukorv
                ////            where item.Nimetus == ValitudToode
                ////            select item.Kogus);
                //Ostukorv = Ostukorv.Where(x => x.Nimetus == ValitudToode)
            }
            else
            {
                MessageBox.Show("Valige midagi!", "Viga");
            }
                    
        }

        private void Osta(object sender, RoutedEventArgs e)
        {
            string Tsekk = "";
            int Summa = 0;
            foreach (var item in Ostukorv)
            {
                Tsekk = Tsekk + "Nimetus: " + item.Nimetus + "\nKogus: " + item.Kogus + "\nHind: " + item.Hind +"EUR\n\n";
                Summa = Summa + item.Kogus * item.Hind;
            }
            Tsekk = Tsekk + "Kokku: " + Summa + "EUR";
            Ostukorv = new List<Toode>();
            MessageBox.Show(Tsekk, "Tšekk");
        }

        private void Hind_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if(tb.Text == "") tb.Text = tb.Name;
        }
    }
}
