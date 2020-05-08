using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class DataMaiorAtualAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var dt = (DateTime)value;
            if (dt.Date >= DateTime.Now.Date)
            {
                return true;
            }
            return false;
        }
    }
}