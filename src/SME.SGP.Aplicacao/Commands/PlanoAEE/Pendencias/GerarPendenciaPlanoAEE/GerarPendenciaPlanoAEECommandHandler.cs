using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPlanoAEECommandHandler : IRequestHandler<GerarPendenciaPlanoAEECommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE;

        public GerarPendenciaPlanoAEECommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaPlanoAEE = repositorioPendenciaPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaPlanoAEE));
        }

        public async Task<bool> Handle(GerarPendenciaPlanoAEECommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AEE, request.UeId, request.Descricao, titulo: request.Titulo));

                    await repositorioPendenciaPlanoAEE.SalvarAsync(new PendenciaPlanoAEE(pendenciaId, request.PlanoAEEId));

                    if(request.Perfil != null)
                        await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, new List<PerfilUsuario> { request.Perfil.Value }));

                    if(request.UsuariosIds != null)
                    {
                        foreach (var usuarioId in request.UsuariosIds)
                            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
                    }
        
                    unitOfWork.PersistirTransacao();

                    if (request.Perfil != null)
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, request.UeId), Guid.NewGuid()));
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }            
            }

            return true;
        }
    }
}
