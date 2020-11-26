using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaExcluirPendenciaAusenciaFechamentoCommandHandler : IRequestHandler<PublicaFilaExcluirPendenciaAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicaFilaExcluirPendenciaAusenciaFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicaFilaExcluirPendenciaAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicaFilaWorkerSgpCommand(RotasRabbit.RotaExecutaExclusaoPendenciasAusenciaFechamento,
                                                       new VerificaExclusaoPendenciasAusenciaFechamentoCommand(request.FechamentoTurmaDisciplinaDto.DisciplinaId,
                                                       request.FechamentoTurmaDisciplinaDto.Bimestre,
                                                       request.FechamentoTurmaDisciplinaDto.Id,
                                                       request.FechamentoTurmaDisciplinaDto.TurmaId,
                                                       request.Usuario.CodigoRf,
                                                       Dominio.TipoPendencia.AusenciaFechamento),
                                                       Guid.NewGuid(),
                                                       request.Usuario));
            return true;
        }
    }
}
