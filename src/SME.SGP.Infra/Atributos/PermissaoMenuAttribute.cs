using System;

namespace SME.SGP.Infra
{
    public class PermissaoMenuAttribute : Attribute
    {
        public PermissaoMenuAttribute()
        {
            EhAlteracao = false;
            EhConsulta = false;
            EhExclusao = false;
            EhInclusao = false;
            EhSubMenu = false;
            EhMenu = true;
        }

        public string Agrupamento { get; set; }
        public bool EhAlteracao { get; set; }
        public bool EhConsulta { get; set; }
        public bool EhExclusao { get; set; }
        public bool EhInclusao { get; set; }
        public bool EhMenu { get; set; }
        public bool EhSubMenu { get; set; }
        public string Icone { get; set; }
        public string Menu { get; set; }
        public int OrdemAgrupamento { get; set; }
        public int OrdemMenu { get; set; }
        public string SubMenu { get; set; }
        public int OrdemSubMenu { get; set; }
        public string Url { get; set; }
    }
}