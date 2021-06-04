using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteRegistroFrequenciaAlunoQuery : IRequest<bool>
    {
        public ExisteRegistroFrequenciaAlunoQuery(long registroFrequenciaId, string codigoAluno, int numeroAula)
        {
            RegistroFrequenciaId = registroFrequenciaId;
            CodigoAluno = codigoAluno;
            NumeroAula = numeroAula;
        }

        public long RegistroFrequenciaId { get; set; }
        public string CodigoAluno { get; set; } 
        public int NumeroAula { get; set; }
    }
}
