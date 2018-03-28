using System;
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

namespace WpfApp
{
    /*
    Looge kassarakendus kasutades WPF raamistikku.

    Müüja saab lisada ise tooteid. Tootel peab olema nimetus, hind ja kogus.
    Müüja saab lisada olemasolevaid tooteid ostukorvi, mille alusel arvutatakse välja summa, mida klient maksma peab. 
    */

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateTooted();
        }

        public void UpdateTooted()
        {
            string[] Tooted = new string[] { };
            bool CheckState = true;

            while (CheckState == true)
            {
                try
                {
                    Tooted = System.IO.File.ReadAllLines("../../../Tooted.txt");
                    CheckState = false;
                }
                catch (System.IO.FileNotFoundException)
                {
                    File.Create("../../../Tooted.txt");
                }
            }

            var TootedList = new List<Toode>();
            int i = 0;
            string TempNimetus = "";
            int TempHind = 0;
            int TempKogus = 0;

            foreach (var item in Tooted)
            {
                i++;
                if (i == 1) TempNimetus = item;
                else if (i == 2) TempHind = int.Parse(item);
                else if (i == 3)
                {
                    TempKogus = int.Parse(item);
                    TootedList.Add(new Toode() { Nimetus = TempNimetus, Hind = TempHind, Kogus = TempKogus });
                }
                else if (i == 4)
                {
                    i = 0;
                    //ToDo: Check
                }
            }

            i = 0;
            TempNimetus = null;
            TempHind = 0;
            TempKogus = 0;

            TootedListBox.ItemsSource = TootedList;
        }
    }
}
