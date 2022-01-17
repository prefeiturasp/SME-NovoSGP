using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioPlanejamentoDiario 
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long AnoLetivo { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int Semestre { get; set; }
        public string CodigoTurma { get; set; }
        public long ComponenteCurricular { get; set; }
        public int Bimestre { get; set; }
        public bool ListarDataFutura { get; set; }
        public bool ExibirDetalhamento { get; set; }
        public string UsuarioNome { get; set; }
        public long[] ComponentesCurricularesDisponiveis { get; set; }
    }
}
