using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoCommand : IRequest
    {
        public SalvarConselhoClasseAlunoCommand(ConselhoClasseAluno conselhoClasseAluno)
        {
            ConselhoClasseAluno = conselhoClasseAluno;
        }

        public ConselhoClasseAluno ConselhoClasseAluno { get; }
    }
}
