using battleship_common;
using System.Collections.Generic;
using System.ServiceModel;


namespace battleship_server
{
    [ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
    public interface IBattleshipService
    {
        [OperationContract(IsOneWay = true)]
        void Join(string name);

        [OperationContract(IsOneWay = true)]//, IsTerminating = true)]
        void Leave(string name, string GUID);

        [OperationContract(IsOneWay = true)]
        void CreateRoom(string name, string GUID);

        [OperationContract(IsOneWay = true)]
        void DeleteRoom(string name, string GUID);

        [OperationContract(IsOneWay = true)]
        void JoinGame(string name, string GUID, string oponent_name);

        [OperationContract(IsOneWay = true)]
        void LeaveGame(string name, string GUID);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string name, string GUID, string message);

        [OperationContract(IsOneWay = true)]
        void ReadyForGame(string name, string GUID, bool []field);

        [OperationContract(IsOneWay = true)]
        void Turn(string name, string GUID, ShootType type, int x, int y);
    }
}


