using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.AulaRecorrenteFake
{
    public class AlterarAulaRecorrenteCommandHandlerFake : IRequestHandler<AlterarAulaRecorrenteCommand, bool>
    {
        public async Task<bool> Handle(AlterarAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
