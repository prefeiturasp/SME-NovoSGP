using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioUsuarios
    {
        public int DreId { get; set; }
        public int UeId { get; set; }
        public string UsuarioRf { get; set; }
        public string[] Perfis { get; set; }
        public int[] Situacoes { get; set; }
        public int DiasSemAcesso { get; set; }
        public bool ExibirHistorico { get; set; }
    }
}
