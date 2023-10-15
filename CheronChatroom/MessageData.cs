using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheronChatroom
{

    public class MessageData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Content {get; set; }
        public string Sender { get; set; }

        public override string ToString() {
            return $"{Content} - {Sender}";
        }
    }
}
