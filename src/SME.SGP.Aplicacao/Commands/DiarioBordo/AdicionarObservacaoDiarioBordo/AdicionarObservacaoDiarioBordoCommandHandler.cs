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
    public class AdicionarObservacaoDiarioBordoCommandHandler : IRequestHandler<AdicionarObservacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;

        public AdicionarObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(AdicionarObservacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordoObservacao = new DiarioBordoObservacao(request.Observacao, request.DiarioBordoId, request.UsuarioId);
            var observacaoId = await repositorioDiarioBordoObservacao.SalvarAsync(diarioBordoObservacao);
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoNovaObservacaoDiarioBordo,
                new NotificarDiarioBordoObservacaoDto(request.DiarioBordoId, usuario, observacaoId), Guid.NewGuid(), null));

            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
