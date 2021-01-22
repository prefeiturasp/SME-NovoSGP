using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioNotificacao
    {
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public long AnoLetivo { get; set; }
        public string DRE { get; set; }
        public string UE { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int Semestre { get; set; }
        public long Turma { get; set; }
        public string UsuarioBuscaNome { get; set; }
        public string UsuarioBuscaRf { get; set; }
        public IEnumerable<long> Categorias { get; set; }
        public IEnumerable<long> Tipos { get; set; }
        public IEnumerable<long> Situacoes { get; set; }
        public bool ExibirDescricao { get; set; }
        public bool ExibirNotificacoesExcluidas { get; set; }

    }
}
