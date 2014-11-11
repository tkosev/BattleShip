using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace battleship_common
{
    [DataContract]
    public class Message
    {
        [DataMember]
        private string message;

        [DataMember]
        private string author;

        [DataMember]
        private DateTime creationTime;

        public Message(string author, DateTime creationTime, string message)
        {
            this.author = author;
            this.message = message;
            this.creationTime = creationTime;
        }

        [DataMember]
        public string Author
        {
            get { return author; }
            set { }
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return creationTime; }
            set { }
        }

        [DataMember]
        public string Text
        {
            get { return message; }
            set { }
        }
    }
}
