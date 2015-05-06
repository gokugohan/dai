using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Azure
{
    public class Log:TableEntity
    {

        public Log() { }
        public Log(string identificador, string conteudo)
        {
            this.PartitionKey = identificador;
            this.RowKey = conteudo;
        }

        public string descricao { get; set; }

        public string toString()
        {
            string ret = "PartitionKey: " + PartitionKey+"<br/>";
            ret += "RowKey: " + RowKey + "<br/>";
            ret += "Descrição: " + descricao+"<br/>";
            return ret;
        }
    }
}
