using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoCommandHandler : IRequestHandler<AlterarObservacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IMediator mediator;

        public AlterarObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator,
                                                          IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
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

            var notificacoes = await repositorioDiarioBordoObservacaoNotificacao.ObterPorDiarioBordoObservacaoId(request.ObservacaoId);

            if (request.Observacao.Trim().Length < 200 && (request.UsuariosIdNotificacao == null || !request.UsuariosIdNotificacao.Any()))
            {          
                var usuariosIdNotificacao = notificacoes.Select(n => n.IdUsuario);
                var usuariosNotificacaoAnterior = usuariosIdNotificacao?.Select(async u => await mediator.Send(new ObterUsuarioPorIdQuery(u)))?.Select(t => t.Result);

                // Excluir Notificação Especifica
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExcluirNotificacaoDiarioBordo,
                      new ExcluirNotificacaoDiarioBordoDto(request.ObservacaoId), Guid.NewGuid(), null));

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo,
                      new NotificarDiarioBordoObservacaoDto(diarioBordoObservacao.DiarioBordoId, request.Observacao, usuario, request.ObservacaoId, usuariosNotificacaoAnterior.Select(u=> u.CodigoRf)), Guid.NewGuid(), null));
            }
            else if (request.UsuariosIdNotificacao != null && request.UsuariosIdNotificacao.Any())
            {
                var usuariosNotificados = notificacoes.Select(n => n.IdUsuario);               
                var usuariosExcluidos = usuariosNotificados.Where(u => !request.UsuariosIdNotificacao.Contains(u) && u != usuario.Id);
                var usuariosNotificacao = request.UsuariosIdNotificacao?.Select(async u => await mediator.Send(new ObterUsuarioPorIdQuery(u)))?.Select(t => t.Result);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo,
                        new NotificarDiarioBordoObservacaoDto(diarioBordoObservacao.DiarioBordoId, request.Observacao, usuario, request.ObservacaoId, usuariosNotificacao.Select(u=> u.CodigoRf)), Guid.NewGuid(), null));

                foreach (var usuarioExcluido in usuariosExcluidos)
                {
                    // Excluir Notificação Especifica
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoAlterarObservacaoDiarioBordo,
                          new AlterarNotificacaoDiarioBordoDto(request.ObservacaoId, usuarioExcluido), Guid.NewGuid(), null));
                }
            }

            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
