using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class DataRequeridaAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return (DateTime)value != DateTime.MinValue;
        }
    }
}