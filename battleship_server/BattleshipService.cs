using battleship_common;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace battleship_server
{
    class Field
    {
        public Field()
        {
            field = new Cell[10, 10];
            Clear();
            
        }
        private Cell[,] field;

        public Cell GetCell(int x, int y)
        {
            return 0 <= x && x < 10 && 0 <= y && y < 10 ? field[x, y] : Cell.Empty;
        }

        public void SetCell(int x, int y, Cell cell_type)
        {
            if (0 <= x && x < 10 && 0 <= y && y < 10)
            {
                field[x, y] = cell_type;
            }
        }
        public void Clear()
        {
            int i, j;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    field[i, j] = Cell.Empty;
                }
            }
        }
    }

    class Point
    {
        public Point()
        {
            x = y = 0;
        }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
    }

    class Game
    {
        public Game(Client opponent)
        {
            this.opponent = opponent;
            this.field = new Field();
            my_turn = false;
            ships = 10;
        }
        private Client opponent;
        public Client Opponent
        {
            get
            {
                return opponent;
            }
        }
        public void SetField(bool []field)
        {
            int pos = 0, i, j;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    this.field.SetCell(j, i, field[pos] ? Cell.Ship : Cell.Empty);
                    pos++;
                }
            }
            ships = 10;
        }
        public Field field;
        public int four_count;
        public int three_count;
        public int two_count;
        public int one_count;
        public bool my_turn;
        public int ships;
    }


    class Client
    {
        private static List<Room> _rooms = new List<Room>();
        private IClientCallback _callback;
        private string _GUID;
        private string _name;
        private Room _room;
        private Game _game;
        private bool _ready;

        public Client(IClientCallback callback, string name)
        {
            this._callback = callback;
            this._name = name;
            this._GUID = Guid.NewGuid().ToString();
            
            callback.LogIn(this._GUID);
            foreach (var room in Client.Rooms)
            {
                callback.RoomCreated(room);
            }
            _room = null;
            _game = null;
            _ready = false;
        }

        public IClientCallback Callback
        {
            get { return _callback; }
        }

        public bool CheckGUID(string GUID)
        {
            return _GUID == GUID;
        }

        public string Name
        {
            get { return _name; }
        }

        public Room CreateRoom()
        {
            if (_room != null || _game != null)
            {
                return null;
            }
            _room = new Room(_name, DateTime.Now);
            _rooms.Add(_room);
            return _room;
        }

        public bool DeleteRoom()
        {
            if (_room == null)
            {
                return false;
            }
            _rooms.Remove(_room);
            _room = null;
            return true;
        }

        public void LeaveGame()
        {
            if (_game != null)
            {
                _game.Opponent.YouCheated();
            }
            _game = null;
        }

        public void YouCheated()
        {
            _game = null;
            try
            {
                Callback.YouCheated();
            }
            catch (Exception) { };
        }

        public void JoinTo(Client opponent)
        {
            _game = opponent.LetsPlay(this);
            _ready = false;
        }

        public Game LetsPlay(Client opponent)
        {
            if (!HaveGame)
            {
                _game = new Game(opponent);
            }
            _ready = false;
            return new Game(this);
        }

        public void SendMessage(string message)
        {
            Message mess = new Message(_name, DateTime.Now, message);
            _game.Opponent.RecieveMessage(mess);
            RecieveMessage(mess);
        }

        public void RecieveMessage(Message mess)
        {
            try
            {
                _callback.TransferMessage(mess);
            }
            catch (Exception) { };
        }

        public void DoTurn(int x, int y)
        {
            if (_game != null)
            {
                if (_game.my_turn)
                {
                    _game.my_turn = false;
                    _game.Opponent.GetTurn(x, y);
                }
            }
        }

        private bool ListContained(List<Point> list, int x, int y)
        {
            foreach (var point in list)
            {
                if (point.x == x && point.y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void GetTurn(int x, int y)
        {
            if (_game != null)
            {
                if (_game.field.GetCell(x, y) == Cell.Empty)
                {
                    _game.field.SetCell(x, y, Cell.Missed);
                    try
                    {
                        _callback.UpdateYourField(x, y, Cell.Missed);
                    }
                    catch (Exception) { }
                    try
                    {
                        _game.Opponent.UpdateOpponentField(x, y, Cell.Missed);
                    }
                    catch (Exception) { }
                    YouTurn();
                    return;
                }
                if (_game.field.GetCell(x, y) == Cell.DeadShip || _game.field.GetCell(x, y) == Cell.Fire || _game.field.GetCell(x, y) == Cell.Missed)
                {
                    _game.Opponent.AlreadyClicked();
                    _game.Opponent.YouTurn();
                    return;
                }
                List<Point> list = new List<Point>();
                list.Add(new Point(x, y));
                _game.field.SetCell(x, y, Cell.Fire);
                bool killed = true;
                for (int i = 0; i < list.Count; i++)
                {
                    int lx = list[i].x;
                    int ly = list[i].y;
                    if (lx > 0)
                    {
                        if (_game.field.GetCell(lx-1, ly) == Cell.Ship)
                        {
                            killed = false;
                            break;
                        }
                        if (_game.field.GetCell(lx-1, ly) == Cell.Fire && !ListContained(list, lx-1, ly))
                        {
                            list.Add(new Point(lx - 1, ly));
                        }
                    }
                    if (lx < 9)
                    {
                        if (_game.field.GetCell(lx + 1, ly) == Cell.Ship)
                        {
                            killed = false;
                            break;
                        }
                        if (_game.field.GetCell(lx + 1, ly) == Cell.Fire && !ListContained(list, lx + 1, ly))
                        {
                            list.Add(new Point(lx + 1, ly));
                        }
                    }
                    if (ly > 0)
                    {
                        if (_game.field.GetCell(lx, y-1) == Cell.Ship)
                        {
                            killed = false;
                            break;
                        }
                        if (_game.field.GetCell(lx, ly-1) == Cell.Fire && !ListContained(list, lx, ly-1))
                        {
                            list.Add(new Point(lx, ly-1));
                        }
                    }
                    if (ly < 9)
                    {
                        if (_game.field.GetCell(lx, ly+1) == Cell.Ship)
                        {
                            killed = false;
                            break;
                        }
                        if (_game.field.GetCell(lx, ly+1) == Cell.Fire && !ListContained(list, lx, ly+1))
                        {
                            list.Add(new Point(lx, ly+1));
                        }
                    }
                }
                if (!killed)
                {
                    try
                    {
                        _callback.UpdateYourField(x, y, Cell.Fire);
                    }
                    catch (Exception) { }
                    try
                    {
                        _game.Opponent.UpdateOpponentField(x, y, Cell.Fire);
                    }
                    catch (Exception) { }
                    _game.Opponent.YouTurn();
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int lx = list[i].x;
                    int ly = list[i].y; 
                    if (lx > 0)
                    {
                        if (_game.field.GetCell(lx - 1, ly) == Cell.Empty)
                        {
                            _game.field.SetCell(lx - 1, ly, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx - 1, ly, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx - 1, ly, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (lx > 0 && ly > 0)
                    {
                        if (_game.field.GetCell(lx - 1, ly-1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx - 1, ly-1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx - 1, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx - 1, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (ly > 0)
                    {
                        if (_game.field.GetCell(lx, ly-1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx, ly-1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (ly > 0 && lx < 9)
                    {
                        if (_game.field.GetCell(lx+1, ly-1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx+1, ly-1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx+1, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx+1, ly-1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (lx < 9)
                    {
                        if (_game.field.GetCell(lx+1, ly) == Cell.Empty)
                        {
                            _game.field.SetCell(lx+1, ly, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx+1, ly, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx+1, ly, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }

                    if (lx < 9 && ly < 9)
                    {
                        if (_game.field.GetCell(lx+1, ly+1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx+1, ly+1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx+1, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx+1, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (ly < 9)
                    {
                        if (_game.field.GetCell(lx, ly+1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx, ly+1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (lx > 0 && ly < 9)
                    {
                        if (_game.field.GetCell(lx-1, ly+1) == Cell.Empty)
                        {
                            _game.field.SetCell(lx-1, ly+1, Cell.Missed);
                            try
                            {
                                _callback.UpdateYourField(lx-1, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                            try
                            {
                                _game.Opponent.UpdateOpponentField(lx-1, ly+1, Cell.Missed);
                            }
                            catch (Exception) { }
                        }
                    }
                    _game.field.SetCell(lx, ly, Cell.DeadShip);
                    try
                    {
                        _callback.UpdateYourField(lx, ly, Cell.DeadShip);
                    }
                    catch (Exception) { }
                    try
                    {
                        _game.Opponent.UpdateOpponentField(lx, ly, Cell.DeadShip);
                    }
                    catch (Exception) { }
                }
                _game.ships--;
                if (_game.ships == 0)
                {
                    try
                    {
                        _game.Opponent.Win();
                    }
                    catch (Exception) { }
                    try
                    {
                        _callback.Loose();
                    }
                    catch (Exception) { }
                    Client client = _game.Opponent;
                    _game = null;
                    client.JoinTo(this);
                    return;
                }
                _game.Opponent.YouTurn();
                return;
            }
        }

        public void UpdateOpponentField(int x, int y, Cell type)
        {
            try
            {
                _callback.UpdateOpponentField(x, y, type);
            }
            catch (Exception) { }
        }

        public void AlreadyClicked()
        {
            try
            {
                _callback.AlreadyClicked();
            }
            catch (Exception) { }
        }

        public void Win()
        {
            try
            {
                _callback.Win();
            }
            catch (Exception) { }
        }

        public void YouTurn()
        {
            _game.my_turn = true;
            try
            {
                _callback.YouTurn();
            }
            catch (Exception) { }
        }

        public void SetField(bool[] field)
        {
            if (_game != null)
            {
                _game.SetField(field);
                _ready = true;
                if (!IsOpponentReady())
                    _game.my_turn = true;
            }
        }

        public bool IsOpponentReady()
        {
            if (_game != null)
            {
                return _game.Opponent.Ready; 
            }
            return false;
        }

        public string Opponent()
        {
            if (_game != null)
            {
                return _game.Opponent.Name;
            }
            return "";
        }

        public static List<Room> Rooms
        {
            get
            {
                return _rooms;
            }
        }

        public bool HaveRoom
        {
            get
            {
                return _room != null;
            }
        }

        public bool HaveGame
        {
            get
            {
                return _game != null;
            }
        }

        public bool Ready
        {
            get
            {
                return _ready;
            }
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BattleshipService : IBattleshipService
    {
        Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();

        public void Join(string name)
        {
            if (!clientsDictionary.ContainsKey(name))
            {
                Client newClient = null;
                try
                {
                    newClient = new Client(OperationContext.Current.GetCallbackChannel<IClientCallback>(), name);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error joining client {0}!", name);
                    return;
                }
                clientsDictionary.Add(name, newClient);
                Console.WriteLine("Client {0} joined!", name);
            }
            else
            {
                Console.WriteLine("Someone wants to get the used login {0}!", name);
                try
                {
                    OperationContext.Current.GetCallbackChannel<IClientCallback>().UserNameExists();
                }
                catch (Exception) { }
            }
        }

        public void CreateRoom(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                Room room = clientsDictionary[name].CreateRoom();
                if (room != null)
                {
                    Console.WriteLine("Client {0} created room!", name);
                    List<Client> failed = new List<Client>();
                    foreach (var client in clientsDictionary.Values)
                    {
                        try
                        {
                            client.Callback.RoomCreated(room);
                        }
                        catch (Exception e)
                        {
                            failed.Add(client);
                        }
                    }
                    SecureDeleteClients(failed);
                    return;
                }
                Console.WriteLine("Client {0} wants to create room, but room already created!", name);
                try
                {
                    clientsDictionary[name].Callback.FatalError("Room already created!");
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to create room!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void Leave(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                SecureDeleteClient(clientsDictionary[name]);
                return;
            }
            Console.WriteLine("Unknown client wants to leave!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        private void SecureDeleteClient(Client client)
        {
            clientsDictionary.Remove(client.Name);
            client.LeaveGame();
            bool deleted = client.DeleteRoom();
            if (deleted)
            {
                Console.WriteLine("Client {0} deleted room!", client.Name);
                List<Client> failed = new List<Client>();
                foreach (var iclient in clientsDictionary.Values)
                {
                    try
                    {
                        iclient.Callback.RoomDeleted(client.Name);
                    }
                    catch (Exception)
                    {
                        failed.Add(client);
                    }
                }
                SecureDeleteClients(failed);
            }
            Console.WriteLine("Client {0} leave!", client.Name);
            return;
        }

        private void SecureDeleteClients(List<Client> clients)
        {
            if (clients.Count == 0)
            {
                return;
            }
            List<Client> failed = new List<Client>();
            foreach (var client in clients)
            {
                clientsDictionary.Remove(client.Name);
            }
            foreach (var client in clients)
            {
                client.LeaveGame();
                bool deleted = client.DeleteRoom();
                if (deleted)
                {
                    Console.WriteLine("Client {0} deleted room!", client.Name);
                    foreach (var iclient in clientsDictionary.Values)
                    {
                        try
                        {
                            iclient.Callback.RoomDeleted(client.Name);
                        }
                        catch (Exception)
                        {
                            if (!failed.Contains(client))
                                failed.Add(client);
                        }
                    }
                    continue;
                }
                Console.WriteLine("Client {0} leave!", client.Name);
            }
            SecureDeleteClients(failed);
        }

        public void DeleteRoom(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                bool deleted = clientsDictionary[name].DeleteRoom();
                if (deleted)
                {
                    Console.WriteLine("Client {0} delete room!", name);
                    List<Client> failed = new List<Client>();
                    foreach (var client in clientsDictionary.Values)
                    {
                        try
                        {
                            client.Callback.RoomDeleted(name);
                        }
                        catch (Exception)
                        {
                            failed.Add(client);
                        }
                    }
                    SecureDeleteClients(failed);
                    return;
                }           
                if (clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} is gaming now!)", name);
                    return;
                }
                Console.WriteLine("Client {0} wants to delete room, but room does not exists!", name);
                try
                {
                    clientsDictionary[name].Callback.FatalError("Room does not exists!");
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to delete room!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void JoinGame(string name, string GUID, string opponent_name)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (clientsDictionary.ContainsKey(opponent_name))
                {
                    if (clientsDictionary[opponent_name].HaveRoom && !clientsDictionary[opponent_name].HaveGame)
                    {
                        clientsDictionary[opponent_name].DeleteRoom();
                        Console.WriteLine("Client {0} delete room!", name);
                        List<Client> failed = new List<Client>();
                        foreach (var client in clientsDictionary.Values)
                        {
                            //if (client.Name == opponent_name)
                            //    continue;
                            try
                            {
                                client.Callback.RoomDeleted(opponent_name);
                            }
                            catch (Exception)
                            {
                                failed.Add(client);
                            }
                        }
                        SecureDeleteClients(failed);
                        try
                        {
                            clientsDictionary[name].Callback.PrepareToGame(opponent_name);
                        }
                        catch (Exception)
                        {
                            SecureDeleteClient(clientsDictionary[name]);
                            return;
                        }
                        clientsDictionary[name].JoinTo(clientsDictionary[opponent_name]);
                        try
                        {
                            clientsDictionary[opponent_name].Callback.PrepareToGame(name);
                        }
                        catch (Exception)
                        {
                            SecureDeleteClient(clientsDictionary[opponent_name]);
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            clientsDictionary[name].Callback.GameNotExists();
                        }
                        catch (Exception)
                        {
                            SecureDeleteClient(clientsDictionary[name]);
                            return;
                        }
                    }
                    return;
                }
                Console.WriteLine("Client {0} wants to join game, but opponent does not exists!", name);
                try
                {
                    clientsDictionary[name].Callback.FatalError("Opponent does not exists!");
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to join game!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void ReadyForGame(string name, string GUID, bool[] field)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (!clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} can't own game!", name);
                    try
                    {
                        clientsDictionary[name].Callback.FatalError("You are not gaming!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                if (field.Length != 100)
                {
                    Console.WriteLine("Client {0} send wrong field (size != 100)!", name);
                    try
                    {
                        clientsDictionary[name].Callback.BadField("Wrong size of field!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                int[] ships_count = new int[5];
                byte[] cell_status = new byte[100];
                for (int i = 0; i < 100; i++)
                {
                    if (field[i] && cell_status[i] == 2)
                    {
                        Console.WriteLine("Client {0} send wrong field (ship touch another ship)!", name, GUID);
                        try
                        {
                            clientsDictionary[name].Callback.BadField("Ship touch another ship!");
                        }
                        catch (Exception)
                        {
                            SecureDeleteClient(clientsDictionary[name]);
                        }
                        return;
                    }
                    if (field[i] && cell_status[i] == 0)
                    {
                        int horizontal_length = 1;
                        cell_status[i] = 1;
                        if (i < 90)
                            cell_status[i + 10] = 2;
                        int j = i+1;
                        while (j % 10 != 0 && field[j])
                        {
                            horizontal_length += 1;
                            cell_status[j] = 1;
                            if (j < 90)
                                cell_status[j + 10] = 2; //2 is ship surroundings
                            j++;
                        }
                        if (j % 10 != 0)
                        {
                            cell_status[j] = 2;
                            if (j < 90)
                                cell_status[j + 10] = 2;
                        }
                        if (horizontal_length > 1)
                        {
                            if (horizontal_length > 4)
                            {
                                Console.WriteLine("Client {0} send ship which horizontal length > 4!", name, GUID);
                                try
                                {
                                    clientsDictionary[name].Callback.BadField("Wrong ship length!");
                                }
                                catch (Exception)
                                {
                                    SecureDeleteClient(clientsDictionary[name]);
                                }
                                return;
                            }
                            ships_count[horizontal_length]++;
                        }
                        else
                        {
                            int vertical_length = 1;
                            cell_status[i] = 1;
                            if (i % 10 != 0)
                                cell_status[i - 1] = 2;
                            if (i % 10 != 9)
                                cell_status[i + 1] = 2;
                            j = i+10;
                            while (j < 100 && field[j])
                            {
                                vertical_length += 1;
                                cell_status[j] = 1;
                                if (j % 10 != 0)
                                    cell_status[j - 1] = 2;
                                if (j % 10 != 9)
                                    cell_status[j + 1] = 2;
                                j += 10;
                            }
                            if (j < 100)
                            {
                                cell_status[j] = 2;
                                if (j % 10 != 0)
                                    cell_status[j - 1] = 2;
                                if (j % 10 != 9)
                                    cell_status[j + 1] = 2;
                            }
                            if (vertical_length > 4)
                            {
                                Console.WriteLine("Client {0} send ship which vertical length > 4!", name, GUID);
                                try
                                {
                                    clientsDictionary[name].Callback.BadField("Wrong ship length!");
                                }
                                catch (Exception)
                                {
                                    SecureDeleteClient(clientsDictionary[name]);
                                }
                                return;
                            }
                            ships_count[vertical_length]++;
                        }
                    }
                }
                if (ships_count[1] != 4)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    try
                    {
                        clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                if (ships_count[2] != 3)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    try
                    {
                        clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                if (ships_count[3] != 2)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    try
                    {
                        clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                if (ships_count[4] != 1)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    try
                    {
                        clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                Console.WriteLine("Client {0} send good field!", name, GUID);
                try
                {
                    clientsDictionary[name].Callback.GoodField();
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                    return;
                }
                clientsDictionary[name].SetField(field);
                if (clientsDictionary[name].IsOpponentReady())
                {
                    try
                    {
                        clientsDictionary[name].Callback.StartGame();
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                        return;
                    }
                    try
                    {
                        clientsDictionary[clientsDictionary[name].Opponent()].Callback.StartGame();
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    try
                    {
                        clientsDictionary[clientsDictionary[name].Opponent()].Callback.YouTurn();
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }

                    return;
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to join game!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception)
            { }
        }

        public void SendMessage(string name, string GUID, string text)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (!clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} not gaming now!)", name);
                    try
                    {
                        clientsDictionary[name].Callback.FatalError("You are not gaming now!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                Console.WriteLine("Client {0} send a message!", name);
                clientsDictionary[name].SendMessage(text);
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to send a message!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void Turn(string name, string GUID, ShootType type, int x, int y)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (!clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} not gaming now!)", name);
                    try
                    {
                        clientsDictionary[name].Callback.FatalError("You are not gaming now!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                Console.WriteLine("Client {0} did a turn!", name);
                clientsDictionary[name].DoTurn(x, y);
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to shoot (everybody wants)!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void LeaveGame(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (!clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} not gaming now!", name);
                    try
                    {
                        clientsDictionary[name].Callback.FatalError("You are not gaming now!");
                    }
                    catch (Exception)
                    {
                        SecureDeleteClient(clientsDictionary[name]);
                    }
                    return;
                }
                Console.WriteLine("Client {0} leaved game!", name);
                clientsDictionary[name].LeaveGame();
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to leave game!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }
    }
}
