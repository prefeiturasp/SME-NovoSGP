using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class AtribuicoesPorTurmaEProfessorDto
    {
        public Modalidade? Modalidade { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string UsuarioRf { get; set; }
        public string UsuarioNome { get; set; }
        public bool? Substituir { get; set; }
        public string DreCodigo { get; set; }
        public string[] TurmaIds { get; set; }
        public int AnoLetivo { get; set; }
        public bool? Historico { get; set; }
    }
}
