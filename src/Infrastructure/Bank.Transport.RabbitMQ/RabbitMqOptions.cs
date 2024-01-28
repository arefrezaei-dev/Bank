using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Transport.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ushort? Port { get; set; }
    }
}
