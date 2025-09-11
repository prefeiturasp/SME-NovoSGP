using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AnotacaoFrequenciaAluno : EntidadeBase
    {
        public AnotacaoFrequenciaAluno()
        {

        }
        public AnotacaoFrequenciaAluno(long aulaId, string anotacao, string codigoAluno, long? motivoAusenciaId = null)
        {
            MotivoAusenciaId = motivoAusenciaId;
            AulaId = aulaId;
            Anotacao = anotacao;
            CodigoAluno = codigoAluno;
        }

        public long? MotivoAusenciaId { get; set; }
        public long AulaId { get; set; }
        public string Anotacao { get; set; }
        public string CodigoAluno { get; set; }
        public bool Excluido { get; set; }
    }
}
