using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    public class ByteArrayTypeHandler : SqlMapper.TypeHandler<byte[]>
    {
        public override void SetValue(IDbDataParameter parameter, byte[] value)
        {
            parameter.Value = Encoding.UTF8.GetString(value, 0, value.Length);
        }

        public override byte[] Parse(object value)
        {
            return Encoding.UTF8.GetBytes((string)value);
        }
    }
}
