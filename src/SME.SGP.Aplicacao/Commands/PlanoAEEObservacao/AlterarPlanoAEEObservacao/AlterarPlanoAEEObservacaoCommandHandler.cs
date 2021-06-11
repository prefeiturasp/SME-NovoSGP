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
    public class AlterarPlanoAEEObservacaoCommandHandler : IRequestHandler<AlterarPlanoAEEObservacaoCommand, AuditoriaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;
        private readonly IMediator mediator;

        public AlterarPlanoAEEObservacaoCommandHandler(IUnitOfWork unitOfWork, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao, IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(AlterarPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            var observacaoPlano = await repositorioPlanoAEEObservacao.ObterPorIdAsync(request.Id);
            observacaoPlano.Observacao = request.Observacao;

            var planoAEE = request.PossuiUsuarios ? await ObterPlanoAEE(request.PlanoAEEId) : null;
            var usuarioAtual = request.PossuiUsuarios ? await mediator.Send(new ObterUsuarioLogadoQuery()) : null;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPlanoAEEObservacao.SalvarAsync(observacaoPlano);

                    await ExcluirNotificacoesExistentes(request.Id);
                    if (request.PossuiUsuarios)
                        await NotificarUsuarios(request.Usuarios, planoAEE, usuarioAtual, request.Id, request.Observacao);

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return (AuditoriaDto)observacaoPlano;
        }

        private async Task ExcluirNotificacoesExistentes(long id)
        {
            await mediator.Send(new ExcluirNotificacaoPlanoAEEObservacaoCommand(id));
        }

        private async Task NotificarUsuarios(IEnumerable<long> usuarios, PlanoAEE planoAEE, Usuario usuarioAtual, long observacaoId, string observacao)
        {
            await mediator.Send(new NotificarObservacaoPlanoAEECommand(observacaoId,
                                                                       planoAEE.Id,
                                                                       usuarioAtual.Nome,
                                                                       usuarioAtual.CodigoRf,
                                                                       planoAEE.AlunoNome,
                                                                       planoAEE.AlunoCodigo,
                                                                       observacao,
                                                                       planoAEE.Turma.Ue.Dre.Abreviacao,
                                                                       $"{planoAEE.Turma.Ue.TipoEscola.ShortName()} {planoAEE.Turma.Ue.Nome}",
                                                                       usuarios));
        }


        private async Task<PlanoAEE> ObterPlanoAEE(long planoAEEId)
            => await mediator.Send(new ObterPlanoAEEComTurmaUeEDrePorIdQuery(planoAEEId));
    }
}
