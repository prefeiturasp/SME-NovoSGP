using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (request.UsuariosIdNotificacao != null && request.UsuariosIdNotificacao.Any())
            {
                var usuariosNotificacao = new List<Usuario>();
                foreach (var item in request.UsuariosIdNotificacao?.Select(a => a))
                {
                    var usuarioSelecionado = await mediator.Send(new ObterUsuarioPorIdQuery(item));
                    usuariosNotificacao.Add(usuarioSelecionado);
                }

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo,
                  new NotificarDiarioBordoObservacaoDto(request.DiarioBordoId, request.Observacao, usuario, observacaoId, usuariosNotificacao), Guid.NewGuid(), null));
            }
            else
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo,
                  new NotificarDiarioBordoObservacaoDto(request.DiarioBordoId, request.Observacao, usuario, observacaoId), Guid.NewGuid(), null));
            }

            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
