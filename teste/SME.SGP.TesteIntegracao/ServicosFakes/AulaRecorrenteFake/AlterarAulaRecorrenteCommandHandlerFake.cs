using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
