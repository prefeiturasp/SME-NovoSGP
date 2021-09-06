using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioAtaBimestralDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string[] TurmasCodigo { get; set; }
        public int? Bimestre { get; set; }
        public int? Semestre { get; set; }
        public bool ExibirHistorico { get; set; }
        public string UsuarioLogadoNome { get; set; }
        public string UsuarioLogadoRf { get; set; }

    }
}