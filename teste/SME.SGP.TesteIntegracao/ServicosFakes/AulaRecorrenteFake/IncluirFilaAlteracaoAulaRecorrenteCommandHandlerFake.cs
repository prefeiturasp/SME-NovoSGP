using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.AulaRecorrenteFake
{
    public class IncluirFilaAlteracaoAulaRecorrenteCommandHandlerFake : IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaAlteracaoAulaRecorrenteCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(IncluirFilaAlteracaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {

            await mediator.Send(new AlterarAulaRecorrenteCommand(request.Usuario,
                                               request.AulaId,
                                               request.DataAula,
                                               request.Quantidade,
                                               request.CodigoTurma,
                                               request.ComponenteCurricularId,
                                               request.NomeComponenteCurricular,
                                               request.TipoCalendarioId,
                                               request.TipoAula,
                                               request.CodigoUe,
                                               request.EhRegencia,
                                               request.RecorrenciaAula));
            return true;
        }
    }
}
