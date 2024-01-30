using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public class
        SalvarConselhoClasseAlunoNotaCommandHandler : IRequestHandler<SalvarConselhoClasseAlunoNotaCommand,
            ConselhoClasseNotaRetornoDto>
    {
        public Task<ConselhoClasseNotaRetornoDto> Handle(SalvarConselhoClasseAlunoNotaCommand request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new ConselhoClasseNotaRetornoDto());
        }
    }
}