using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaDiarioPorAulaIdCommandHandler : IRequestHandler<ExcluirPendenciaDiarioPorAulaIdCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;
        private readonly IRepositorioPendencia repositorioPendencia;
        public ExcluirPendenciaDiarioPorAulaIdCommandHandler(IMediator mediator, IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo, IRepositorioPendencia repositorioPendencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }
        public async Task<bool> Handle(ExcluirPendenciaDiarioPorAulaIdCommand request, CancellationToken cancellationToken)
        {
            var pendenciasEUsuarios = await mediator.Send(new ObterIdPendenciaDiarioBordoPorAulaIdQuery(request.AulaId), cancellationToken);

            if (pendenciasEUsuarios.Any())
            {
                await repositorioPendenciaDiarioBordo.ExcluirPorAulaId(request.AulaId);

                foreach (var pendencia in pendenciasEUsuarios)
                {
                    await mediator.Send(new ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand(pendencia.PendenciaId, pendencia.UsuarioId));

                    var existePendencia = await mediator.Send(new VerificaSeExistePendenciaDiarioComPendenciaIdQuery(pendencia.PendenciaId), cancellationToken);

                    if (!existePendencia)
                        await repositorioPendencia.ExclusaoLogicaPendencia(pendencia.PendenciaId);
                }

                return true;
            }
            return false;
        }
    }
}

