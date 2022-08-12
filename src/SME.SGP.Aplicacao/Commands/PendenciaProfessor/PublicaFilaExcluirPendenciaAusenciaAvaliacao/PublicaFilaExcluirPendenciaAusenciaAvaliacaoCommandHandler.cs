using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommandHandler : IRequestHandler<PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAusenciaAvaliacao,
                                                       new VerificaExclusaoPendenciasAusenciaAvaliacaoCommand(request.TurmaCodigo,
                                                                                                              request.ComponentesCurriculares,
                                                                                                              Dominio.TipoPendencia.AusenciaDeAvaliacaoProfessor,
                                                                                                              request.DataAvaliacao),
                                                       Guid.NewGuid(),
                                                       request.Usuario));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAusenciaAvaliacao,
                                                       new VerificaExclusaoPendenciasAusenciaAvaliacaoCommand(request.TurmaCodigo,
                                                                                                              request.ComponentesCurriculares,
                                                                                                              Dominio.TipoPendencia.AusenciaDeAvaliacaoCP,
                                                                                                              request.DataAvaliacao),
                                                       Guid.NewGuid(),
                                                       request.Usuario));

            return true;
        }
    }
}
