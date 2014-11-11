using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using battleship_common;

namespace battleship_client
{
    /// <summary>
    /// Interaction logic for RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : UserControl
    {
        private ObservableCollection<Room> _rooms = new ObservableCollection<Room>();
        private Main main;

        public RoomsPage(Main main)
        {
            this.main = main;
            this.DataContext = _rooms;
            //Room room = new Room("ya", DateTime.Now);
            InitializeComponent();
            //rooms.Add(room);
        }

        public void SetUsername(string username)
        {
            welcome.Content = "Welcome, " + username + "!";
        }

        public void ResetButtons()
        {
            joinButton.IsEnabled = true;
            createButton.IsEnabled = true;
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }

        public void DeleteRoom(string name)
        {
            IEnumerable<Room> rooms =
                from room in _rooms
                where room.Name == name
                select room;
            if (rooms.Count<Room>() > 0)
            {
                _rooms.Remove(rooms.First<Room>());
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(((Room)((DataGrid)sender).CurrentCell.Item).Name, "Good", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = (Room)(roomsGrid.SelectedItem);
            if (room != null)
            {
                createButton.IsEnabled = false;
                joinButton.IsEnabled = false;
                //MessageBox.Show(room.Name, "Good", MessageBoxButton.OK, MessageBoxImage.Error);
                main.JoinGame(room.Name);
            }
            else
            {
                MessageBox.Show("First you need to select the room!", "Try again...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            createButton.IsEnabled = false;
            joinButton.IsEnabled = false;
            main.CreateRoom();
        }


    }
}
