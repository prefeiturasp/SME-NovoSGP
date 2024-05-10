﻿using SME.SGP.Dominio;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class ListaTemElementosAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list.NaoEhNulo())
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}