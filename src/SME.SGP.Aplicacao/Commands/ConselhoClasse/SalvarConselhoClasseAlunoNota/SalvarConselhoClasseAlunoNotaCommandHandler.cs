using MediatR;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

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