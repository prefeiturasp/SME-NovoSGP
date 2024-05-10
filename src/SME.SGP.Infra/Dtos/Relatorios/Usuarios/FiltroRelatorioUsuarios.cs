using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioUsuarios
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string UsuarioRf { get; set; }
        public string[] Perfis { get; set; }
        public int[] Situacoes { get; set; }
        public int DiasSemAcesso { get; set; }
        public bool ExibirHistorico { get; set; }
        public string NomeUsuario { get; set; }
    }
}
