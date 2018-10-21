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

namespace EncryptSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<int> rand = new List<int>();
        Random random;
        
        const int LETTERCOUNT = 26;

        public string split(string phrase)
        {
            string[] words = phrase.Split(' ');
            string retString = ""; 

            for (int i = 0; i < words.Length; i++)
            {
                if (Encryption.IsChecked == true)
                {
                    retString += encrypt(words[i]) + " ";
                }

                if (Decryption.IsChecked == true)
                {
                    retString += decrypt(words[i]) + " ";
                }
            }

            return retString;
        }
        
        public string encrypt(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (i < rand.Count)
                {
                    rand[i] = random.Next(0, LETTERCOUNT);
                }

                else 
                {
                    rand.Add(random.Next(0, LETTERCOUNT));
                }
            }

            char[] cArray = word.ToCharArray();
            byte[] bArray = Encoding.ASCII.GetBytes(cArray);
            byte[] retArray = bArray;

            for (int i = 0; i < word.Length; i++)
            {
                byte asciiCharCode = char.IsUpper(cArray[i]) ? (byte)'A' : (byte)'a';

                retArray[i] += (byte)rand[i];
                
                if (!(bArray[i] <= LETTERCOUNT - 1 + asciiCharCode))
                {
                    retArray[i] = (byte)(((bArray[i] - asciiCharCode) % LETTERCOUNT) + asciiCharCode);
                }
            }

            return new string(Encoding.ASCII.GetChars(retArray));
        }
        
        
        public string decrypt(string word)
        {
                for (int i = 0; i < word.Length; i++)
                {
                    if (i < rand.Count)
                    {
                        rand[i] = random.Next(0, LETTERCOUNT);
                    }

                    else
                    {
                        rand.Add(random.Next(0, LETTERCOUNT));
                    }
                }
            
            char[] cArray = word.ToCharArray();
            byte[] bArray = Encoding.ASCII.GetBytes(cArray);
            byte[] retArray = bArray;

            for (int i = 0; i < word.Length; i++)
            {
                byte asciiCharCode = char.IsUpper(cArray[i]) ? (byte)'A' : (byte)'a';

                if (bArray[i] - asciiCharCode >= rand[i])
                {
                    retArray[i] -= (byte)rand[i];
                }

                else
                {
                    retArray[i] = (byte)(retArray[i] + LETTERCOUNT - rand[i]);
                }
            }

            return new string(Encoding.ASCII.GetChars(retArray));
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Seed.Text == null || Seed.Text == "")
            {
                MessageBox.Show("No seed has been specified and the program will use the default seed");
                random = new Random(1);
            }

            else
            {
                int seed;

                if (int.TryParse(Seed.Text, out seed))
                {
                    MessageBox.Show("The program will use the specified seed: " + seed);
                    random = new Random(seed);
                }

                else
                {
                    MessageBox.Show("The specified seed is invalid and the program will use the default seed");
                    random = new Random(1);
                }
            }
            if (Encryption.IsChecked == true || Decryption.IsChecked == true)
            {
                lol.Text = split(Box.Text);
            }

            else 
            {
                MessageBox.Show("Error: No option is selected.");
            }
        }

        private void Encryption_Checked(object sender, RoutedEventArgs e)
        {
            button.Content = "Encrypt";
        }

        private void Decryption_Checked(object sender, RoutedEventArgs e)
        {
            button.Content = "Decrypt";
        }
    }
}
