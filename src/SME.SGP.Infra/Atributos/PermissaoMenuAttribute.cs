using System;

namespace SME.SGP.Infra
{
    public class PermissaoMenuAttribute : Attribute
    {
        public string Agrupamento { get; set; }
        public string Descricao { get; set; }
        public string Icone { get; set; }
        public string Url { get; set; }
    }
}