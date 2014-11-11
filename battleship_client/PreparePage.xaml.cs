using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace battleship_client
{

    class SmartField
    {
        public SmartField()
        {
            field = new bool[100];
            for (int i = 0; i < 100; i++)
            {
                field[i] = false;
            }
        }
        private bool[] field;
        public void Set(int x, int y)
        {
            if (x >= 0 && x < 10 && y >= 0 && y < 10)
            {
                field[x + y * 10] = true;
            }
        }
        public bool Get(int x, int y)
        {
            if (x >= 0 && x < 10 && y >= 0 && y < 10)
            {
                return field[x + y * 10];
            }
            return true;
        }
    }


    public partial class PreparePage : UserControl
    {
        private Main main;
        private bool []field;
        private bool ready;
        private string opponent_name;
        public PreparePage(Main main, string opponent_name, string initial_chat)
        {
            InitializeComponent();
            this.main = main;
            field = new bool[100];
            Greetings.Content = "Please, set you ships. You are playing with " + opponent_name;
            textBlockMessages.Text = initial_chat;
            ready = false;
            this.opponent_name = opponent_name;
        }

        public void PostMessage(string message)
        {
            textBlockMessages.Text = textBlockMessages.Text + message + "\n";
        }

        public void Retry()
        {
        }

        private void Cell_LeftClick(object sender, RoutedEventArgs e)
        {
            if (ready)
                return;
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = true;
            Style style = this.FindResource("FilledCell") as Style;
            cell.Style = style;
        }

        private void Cell_RightClick(object sender, RoutedEventArgs e)
        {
            if (ready)
                return;
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = false;
            Style style = this.FindResource("EmptyCell") as Style;
            cell.Style = style;
        }

        private void Cell_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ready)
                return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle cell = sender as Rectangle;
                field[int.Parse(cell.Name.Remove(0, 4))] = true;
                Style style = this.FindResource("FilledCell") as Style;
                cell.Style = style;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Rectangle cell = sender as Rectangle;
                field[int.Parse(cell.Name.Remove(0, 4))] = false;
                Style style = this.FindResource("EmptyCell") as Style;
                cell.Style = style;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.LeaveGame();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (messageInput.Text.Length > 0 || !string.IsNullOrEmpty(messageInput.Text))
            {
                main.SendMessage(messageInput.Text);
                messageInput.Clear();
            }
            else
                MessageBox.Show("Message is empty...", "Try again...", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ready = true;
            ReadyButton.IsEnabled = false;
            RandomButton.IsEnabled = false;
            main.CheckField(field);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Random rand = new Random((int)DateTimeOffset.Now.UtcTicks);
            int ship_length = 4;
            for (int i = 0; i < 100; i++)
            {
                if (field[i])
                {
                    Rectangle cell = (Rectangle)FindName(string.Format("cell{0}", i));
                    cell.Style = this.FindResource("EmptyCell") as Style;
                    field[i] = false;
                }
            }
            SmartField sf = new SmartField();
            while (ship_length > 0)
            {
                for (int i = 0; i < 5 - ship_length; i++)
                {
                    int dx = 0, dy = 0, maxx = 10, maxy = 10;
                    if (rand.Next() % 2 == 0)
                    {
                        dx = 1;
                        maxx = 10 - ship_length + 1;
                    }
                    else
                    {
                        dy = 1;
                        maxy = 10 - ship_length + 1;
                    }
                    bool placed = false;
                    while (!placed)
                    {
                        int x = rand.Next(maxx);
                        int y = rand.Next(maxy);
                        bool empty = true;
                        for (int j = 0; j < ship_length; j++)
                        {
                            if (sf.Get(x + dx * j, y + dy * j))
                            {
                                empty = false;
                            }
                        }
                        if (empty)
                        {
                            placed = true;
                            for (int j = 0; j < ship_length; j++)
                            {
                                int place = x + dx * j + (y + dy * j) * 10;
                                field[place] = true;
                                Rectangle cell = (Rectangle)FindName(string.Format("cell{0}", place));
                                cell.Style = this.FindResource("FilledCell") as Style;
                            }
                            int k, l;
                            for (k = x - 1; k <= x + dx * (ship_length-1) + 1; k++)
                            {
                                for (l = y - 1; l <= y + dy * (ship_length-1) + 1; l++)
                                {
                                    sf.Set(k, l);
                                }
                            }
                        }
                    }
                }
                ship_length--;
            }
        }

        public void GoodField()
        {
            MessageBox.Show("Please, wait your opponent!", "Good!", MessageBoxButton.OK, MessageBoxImage.Information);
            Greetings.Content = "Good! Wait your opponent. You are playing with " + opponent_name;
        }

        public void BadField(string message)
        {
            MessageBox.Show(message, "Wrong placements...", MessageBoxButton.OK, MessageBoxImage.Warning);
            ready = false;
            ReadyButton.IsEnabled = true;
            RandomButton.IsEnabled = true;
        }

        public GamePage GetGamePage()
        {
            return new GamePage(main, opponent_name, textBlockMessages.Text, field);

        }
    }
}
