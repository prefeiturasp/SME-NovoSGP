using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRegistroFrequenciaAlunoCommand : IRequest<long>
    {
        public SalvarRegistroFrequenciaAlunoCommand(RegistroFrequenciaAluno registroFrequenciaAluno)
        {
            RegistroFrequenciaAluno = registroFrequenciaAluno;
        }

        public RegistroFrequenciaAluno RegistroFrequenciaAluno { get; set; }
    }
}
