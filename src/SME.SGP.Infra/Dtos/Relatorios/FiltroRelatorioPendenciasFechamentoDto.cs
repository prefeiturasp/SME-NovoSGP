using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioPendenciasFechamentoDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string[] TurmasCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public int? Bimestre { get; set; }
        public bool ExibirDetalhamento { get; set; }
        public int? Semestre { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}