using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarPlanoAEEObservacaoCommandHandler : IRequestHandler<CriarPlanoAEEObservacaoCommand, AuditoriaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;
        private readonly IMediator mediator;

        public CriarPlanoAEEObservacaoCommandHandler(IUnitOfWork unitOfWork, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao, IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(CriarPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            var observacao = new PlanoAEEObservacao(request.PlanoAEEId, request.Observacao);
            var planoAEE = request.PossuiUsuarios ? await ObterPlanoAEE(request.PlanoAEEId) : null;
            var usuarioAtual = request.PossuiUsuarios ? await mediator.Send(ObterUsuarioLogadoQuery.Instance) : null;

            if (request.PossuiUsuarios)
                request.TratarUsuariosNotificacao(usuarioAtual.Id);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var observacaoId = await repositorioPlanoAEEObservacao.SalvarAsync(observacao);
                    if (request.PossuiUsuarios)
                        await NotificarUsuarios(request.Usuarios, planoAEE, usuarioAtual, observacaoId, request.Observacao);

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }                
            }

            return (AuditoriaDto)observacao;
        }

        private async Task NotificarUsuarios(IEnumerable<long> usuarios, PlanoAEE planoAEE, Usuario usuarioAtual, long observacaoId, string observacao)
        {
            await mediator.Send(new NotificarObservacaoPlanoAEECommand(observacaoId,
                                                                       planoAEE,
                                                                       usuarioAtual,
                                                                       observacao,
                                                                       usuarios));
        }


        private async Task<PlanoAEE> ObterPlanoAEE(long planoAEEId)
            => await mediator.Send(new ObterPlanoAEEComTurmaUeEDrePorIdQuery(planoAEEId));
    }
}
