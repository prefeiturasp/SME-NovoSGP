using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroHistoricoEscolarDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string TurmaCodigo { get; set; }
        public string[] AlunosCodigo { get; set; }
        public bool ImprimirDadosResponsaveis { get; set; }
        public bool PreencherDataImpressao { get; set; }
        public Usuario Usuario { get; set; }
    }
}
