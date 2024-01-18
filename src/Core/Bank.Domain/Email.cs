using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public record Email
    {

        #region Constructor
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            if (!value.Contains('@'))
                throw new ArgumentException($"invalid email address: '{value}'", nameof(value));
            this.Value = value;
        }
        #endregion

        #region Properties
        public string Value { get; }
        #endregion

        public override string ToString()
           => this.Value;

    }
}
