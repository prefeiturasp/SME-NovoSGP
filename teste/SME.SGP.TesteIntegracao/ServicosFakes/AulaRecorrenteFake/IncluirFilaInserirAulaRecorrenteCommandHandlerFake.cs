using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class IncluirFilaInserirAulaRecorrenteCommandHandlerFake : IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaInserirAulaRecorrenteCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaInserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new InserirAulaRecorrenteCommand(request.Usuario,
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
