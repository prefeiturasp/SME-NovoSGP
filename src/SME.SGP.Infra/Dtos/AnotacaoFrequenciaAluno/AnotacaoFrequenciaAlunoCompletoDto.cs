using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AnotacaoFrequenciaAlunoCompletoDto
    {
        public long Id { get; set; }
        public long MotivoAusenciaId { get; set; }
        public MotivoAusenciaDto MotivoAusencia { get; set; }
        public string Anotacao { get; set; }
        public long AulaId { get; set; }
        public string CodigoAluno { get; set; }
        public AlunoDadosBasicosDto Aluno { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
