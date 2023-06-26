using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AutenticacaoFrequenciaSGADto
    {
        public string Rf { get; set; }
        public string ComponenteCurricularCodigo { get; set; } 
        public Turma Turma { get; set; }
    }
}
