using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CodigosAlunosComRegistroFrequenciaAlunoQuery : IRequest<IEnumerable<string>>
    {
        public CodigosAlunosComRegistroFrequenciaAlunoQuery(long registroFrequenciaId, string[] codigosAlunos, int numeroAula)
        {
            RegistroFrequenciaId = registroFrequenciaId;
            CodigosAlunos = codigosAlunos;
            NumeroAula = numeroAula;
        }

        public long RegistroFrequenciaId { get; set; }
        public string[] CodigosAlunos { get; set; }
        public int NumeroAula { get; set; }
    }
}
