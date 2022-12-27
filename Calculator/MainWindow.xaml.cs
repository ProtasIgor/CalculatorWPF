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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isNumberOnEnd = true;
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }
        public void Init()
        {
            int x = 1;
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button button = new Button();
                    button.HorizontalContentAlignment = HorizontalAlignment.Center;
                    button.VerticalContentAlignment = VerticalAlignment.Center;

                    button.Content = $"{x}";
                    button.FontSize = 30;
                    

                    button.SetValue(Grid.RowProperty, i);
                    button.SetValue(Grid.ColumnProperty, j);

                    button.Click += Button_Click;

                    MainGrid.Children.Add(button);

                    x++;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            Text_Box_Main.Text += bt.Content.ToString();

            this.isNumberOnEnd = true;
        }
        private void Button_Click_ToAddPoint(object sender, RoutedEventArgs e)
        {
            if (!this.isNumberOnEnd || Text_Box_Main.Text.Length == 0)
            {
                MessageBox.Show("Отсутствует целая часть", "Error");
                return;
            }
            Text_Box_Main.Text += ".";

            this.isNumberOnEnd = false;
        }
        private void Button_Click_ToClearTextBox(object sender, RoutedEventArgs e)
        {
            Text_Box_Main.Text = string.Empty;

            this.isNumberOnEnd = true;
        }
        private void Button_Click_ToClearOneSimbol(object sender, RoutedEventArgs e)
        {
            if (Text_Box_Main.Text.Length > 0)
                Text_Box_Main.Text = Text_Box_Main.Text.Remove(Text_Box_Main.Text.Length - 1);
            if (Text_Box_Main.Text.Length == 0)
                this.isNumberOnEnd = true;
            else if (Text_Box_Main.Text[^1] >= 0 && Text_Box_Main.Text[^1] < 10)
                this.isNumberOnEnd = true;

        }
        private void Button_Click_Move(object sender, RoutedEventArgs e)
        {
            // throw Exeption
            if (Text_Box_Main.Text.Length == 0)
            {
                return;
            }

            if    (Text_Box_Main.Text[^1] == '+' || Text_Box_Main.Text[^1] == '-'
                || Text_Box_Main.Text[^1] == '*' || Text_Box_Main.Text[^1] == '/')
            {
                Text_Box_Main.Text = Text_Box_Main.Text.Remove(Text_Box_Main.Text.Length - 1, 1);
                Text_Box_Main.Text += ((Button)sender)?.Content?.ToString();
                this.isNumberOnEnd = false;
                return;
            }
            string? value = ((Button)sender)?.Content?.ToString();
            Text_Box_Main.Text += value;
            this.isNumberOnEnd = false;
        }
        private void Button_Click_ToCheckResult(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int count = 0;
            for (int i = 0; i < Text_Box_Main.Text.Length; i++)
            {
                if (Text_Box_Main.Text[i] == '+' || Text_Box_Main.Text[i] == '-' ||
                    Text_Box_Main.Text[i] == '*' || Text_Box_Main.Text[i] == '/')
                {
                    index = i;
                    count++;
                }
                if (count > 1)
                {
                    MessageBox.Show("Данный калькулятор может выполнять только одну операцию за раз.", "Error");
                    Text_Box_Main.Text = Text_Box_Main.Text.Remove(index).ToString();
                    return;
                }
            }
            if (index == -1)
                return;
            
            switch (Text_Box_Main.Text[index])
            {
                case '+':
                    Text_Box_Main.Text = (decimal.Parse(Text_Box_Main.Text[..index]) +
                        decimal.Parse(Text_Box_Main.Text[++index..])).ToString();
                    break;
                case '-':
                    Text_Box_Main.Text = (decimal.Parse(Text_Box_Main.Text[..index]) -
                        decimal.Parse(Text_Box_Main.Text[++index..])).ToString();
                    break;
                case '*':
                    Text_Box_Main.Text = (decimal.Parse(Text_Box_Main.Text[..index]) *
                        decimal.Parse(Text_Box_Main.Text[++index..])).ToString();
                    break;
                case '/':
                    if (decimal.Parse(Text_Box_Main.Text[++index..]) == 0)
                    {
                        MessageBox.Show("На ноль делить нельзя.", "Error");
                        Text_Box_Main.Text = Text_Box_Main.Text.Remove(index).ToString();
                        return;
                    }
                    Text_Box_Main.Text = (decimal.Parse(Text_Box_Main.Text[..index]) /
                        decimal.Parse(Text_Box_Main.Text[++index..])).ToString();
                    break;
                default:
                    MessageBox.Show("Неизвестная ошибка.", "Error");
                    break;
            }
        }
    }
}
