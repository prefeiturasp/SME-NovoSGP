using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoCommandHandler : IRequestHandler<AlterarObservacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;

        public AlterarObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(AlterarObservacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordoObservacao = await repositorioDiarioBordoObservacao.ObterPorIdAsync(request.ObservacaoId);
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            if (diarioBordoObservacao == null)
                throw new NegocioException("Observação do diário de bordo não encontrada.");

            diarioBordoObservacao.ValidarUsuarioAlteracao(request.UsuarioId);

            diarioBordoObservacao.Observacao = request.Observacao;            

            await repositorioDiarioBordoObservacao.SalvarAsync(diarioBordoObservacao);
            if(request.Observacao.Trim().Length < 200)
            {
                // Excluir Notificação Especifica
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaExcluirNotificacaoDiarioBordo,
                      new ExcluirNotificacaoDiarioBordoDto(request.ObservacaoId), Guid.NewGuid(), null));

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoNovaObservacaoDiarioBordo,
                new NotificarDiarioBordoObservacaoDto(diarioBordoObservacao.DiarioBordoId, request.Observacao, usuario, request.ObservacaoId), Guid.NewGuid(), null));
            }

            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
