using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaDiarioBordoCommandHandler : IRequestHandler<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;
        private readonly IRepositorioPendencia repositorioPendencia;
        public ExcluirPendenciaDiarioBordoCommandHandler(IMediator mediator, IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo, IRepositorioPendencia repositorioPendencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }
        public async Task<bool> Handle(ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var pendenciaId = await mediator.Send(new ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery(request.AulaId, request.ComponenteCurricularId), cancellationToken);

            if (pendenciaId > 0)
            {
                await repositorioPendenciaDiarioBordo.Excluir(request.AulaId, request.ComponenteCurricularId);

                var existePendencia = await mediator.Send(new VerificaSeExistePendenciaDiarioComPendenciaIdQuery(pendenciaId), cancellationToken);

                if (!existePendencia)
                {
                    await mediator.Send(new ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand(pendenciaId, usuario.Id));
                    await repositorioPendencia.ExclusaoLogicaPendencia(pendenciaId);
                }                    

                return true;
            }
            return false;
        }
    }
}
