﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace battleship_client.BattleshipServerRef {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/battleship_common")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AuthorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreationTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TextField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string author1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime creationTime1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string messageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Author {
            get {
                return this.AuthorField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthorField, value) != true)) {
                    this.AuthorField = value;
                    this.RaisePropertyChanged("Author");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreationTime {
            get {
                return this.CreationTimeField;
            }
            set {
                if ((this.CreationTimeField.Equals(value) != true)) {
                    this.CreationTimeField = value;
                    this.RaisePropertyChanged("CreationTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text {
            get {
                return this.TextField;
            }
            set {
                if ((object.ReferenceEquals(this.TextField, value) != true)) {
                    this.TextField = value;
                    this.RaisePropertyChanged("Text");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="author")]
        public string author1 {
            get {
                return this.author1Field;
            }
            set {
                if ((object.ReferenceEquals(this.author1Field, value) != true)) {
                    this.author1Field = value;
                    this.RaisePropertyChanged("author1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="creationTime")]
        public System.DateTime creationTime1 {
            get {
                return this.creationTime1Field;
            }
            set {
                if ((this.creationTime1Field.Equals(value) != true)) {
                    this.creationTime1Field = value;
                    this.RaisePropertyChanged("creationTime1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string message {
            get {
                return this.messageField;
            }
            set {
                if ((object.ReferenceEquals(this.messageField, value) != true)) {
                    this.messageField = value;
                    this.RaisePropertyChanged("message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Cell", Namespace="http://schemas.datacontract.org/2004/07/battleship_common")]
    public enum Cell : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Empty = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ship = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fire = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DeadShip = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missed = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BattleshipServerRef.IBattleshipService", CallbackContract=typeof(battleship_client.BattleshipServerRef.IBattleshipServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IBattleshipService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Join")]
        void Join(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Join")]
        System.Threading.Tasks.Task JoinAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Leave")]
        void Leave(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Leave")]
        System.Threading.Tasks.Task LeaveAsync(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/CreateRoom")]
        void CreateRoom(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/CreateRoom")]
        System.Threading.Tasks.Task CreateRoomAsync(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/DeleteRoom")]
        void DeleteRoom(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/DeleteRoom")]
        System.Threading.Tasks.Task DeleteRoomAsync(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/JoinGame")]
        void JoinGame(string name, string GUID, string oponent_name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/JoinGame")]
        System.Threading.Tasks.Task JoinGameAsync(string name, string GUID, string oponent_name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/LeaveGame")]
        void LeaveGame(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/LeaveGame")]
        System.Threading.Tasks.Task LeaveGameAsync(string name, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/SendMessage")]
        void SendMessage(string name, string GUID, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/SendMessage")]
        System.Threading.Tasks.Task SendMessageAsync(string name, string GUID, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/ReadyForGame")]
        void ReadyForGame(string name, string GUID, bool[] field);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/ReadyForGame")]
        System.Threading.Tasks.Task ReadyForGameAsync(string name, string GUID, bool[] field);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Turn")]
        void Turn(string name, string GUID, battleship_common.ShootType type, int x, int y);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Turn")]
        System.Threading.Tasks.Task TurnAsync(string name, string GUID, battleship_common.ShootType type, int x, int y);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBattleshipServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/LogIn")]
        void LogIn(string GUID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/UserNameExists")]
        void UserNameExists();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/RoomCreated")]
        void RoomCreated(battleship_common.Room room);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/RoomDeleted")]
        void RoomDeleted(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/FatalError")]
        void FatalError(string error);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/GameNotExists")]
        void GameNotExists();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/PrepareToGame")]
        void PrepareToGame(string opponent_name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/TransferMessage")]
        void TransferMessage(battleship_client.BattleshipServerRef.Message mess);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/GoodField")]
        void GoodField();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/BadField")]
        void BadField(string comment);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/StartGame")]
        void StartGame();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/YouTurn")]
        void YouTurn();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/UpdateYourField")]
        void UpdateYourField(int x, int y, battleship_client.BattleshipServerRef.Cell state);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/AlreadyClicked")]
        void AlreadyClicked();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/UpdateOpponentField")]
        void UpdateOpponentField(int x, int y, battleship_client.BattleshipServerRef.Cell state);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Win")]
        void Win();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/Loose")]
        void Loose();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBattleshipService/YouCheated")]
        void YouCheated();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBattleshipServiceChannel : battleship_client.BattleshipServerRef.IBattleshipService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BattleshipServiceClient : System.ServiceModel.DuplexClientBase<battleship_client.BattleshipServerRef.IBattleshipService>, battleship_client.BattleshipServerRef.IBattleshipService {
        
        public BattleshipServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public BattleshipServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public BattleshipServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BattleshipServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BattleshipServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Join(string name) {
            base.Channel.Join(name);
        }
        
        public System.Threading.Tasks.Task JoinAsync(string name) {
            return base.Channel.JoinAsync(name);
        }
        
        public void Leave(string name, string GUID) {
            base.Channel.Leave(name, GUID);
        }
        
        public System.Threading.Tasks.Task LeaveAsync(string name, string GUID) {
            return base.Channel.LeaveAsync(name, GUID);
        }
        
        public void CreateRoom(string name, string GUID) {
            base.Channel.CreateRoom(name, GUID);
        }
        
        public System.Threading.Tasks.Task CreateRoomAsync(string name, string GUID) {
            return base.Channel.CreateRoomAsync(name, GUID);
        }
        
        public void DeleteRoom(string name, string GUID) {
            base.Channel.DeleteRoom(name, GUID);
        }
        
        public System.Threading.Tasks.Task DeleteRoomAsync(string name, string GUID) {
            return base.Channel.DeleteRoomAsync(name, GUID);
        }
        
        public void JoinGame(string name, string GUID, string oponent_name) {
            base.Channel.JoinGame(name, GUID, oponent_name);
        }
        
        public System.Threading.Tasks.Task JoinGameAsync(string name, string GUID, string oponent_name) {
            return base.Channel.JoinGameAsync(name, GUID, oponent_name);
        }
        
        public void LeaveGame(string name, string GUID) {
            base.Channel.LeaveGame(name, GUID);
        }
        
        public System.Threading.Tasks.Task LeaveGameAsync(string name, string GUID) {
            return base.Channel.LeaveGameAsync(name, GUID);
        }
        
        public void SendMessage(string name, string GUID, string message) {
            base.Channel.SendMessage(name, GUID, message);
        }
        
        public System.Threading.Tasks.Task SendMessageAsync(string name, string GUID, string message) {
            return base.Channel.SendMessageAsync(name, GUID, message);
        }
        
        public void ReadyForGame(string name, string GUID, bool[] field) {
            base.Channel.ReadyForGame(name, GUID, field);
        }
        
        public System.Threading.Tasks.Task ReadyForGameAsync(string name, string GUID, bool[] field) {
            return base.Channel.ReadyForGameAsync(name, GUID, field);
        }
        
        public void Turn(string name, string GUID, battleship_common.ShootType type, int x, int y) {
            base.Channel.Turn(name, GUID, type, x, y);
        }
        
        public System.Threading.Tasks.Task TurnAsync(string name, string GUID, battleship_common.ShootType type, int x, int y) {
            return base.Channel.TurnAsync(name, GUID, type, x, y);
        }
    }
}