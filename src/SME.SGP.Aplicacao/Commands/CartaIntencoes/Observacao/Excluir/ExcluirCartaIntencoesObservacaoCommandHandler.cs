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
    public class ExcluirCartaIntencoesObservacaoCommandHandler : IRequestHandler<ExcluirCartaIntencoesObservacaoCommand, bool>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;
        private readonly IMediator mediator;

        public ExcluirCartaIntencoesObservacaoCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao, IMediator mediator)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = await repositorioCartaIntencoesObservacao.ObterPorIdAsync(request.ObservacaoId);
            if (cartaIntencoesObservacao == null)
                throw new NegocioException("Observação da carta de intenções não encontrada.");

            cartaIntencoesObservacao.ValidarUsuarioAlteracao(request.UsuarioId);
            cartaIntencoesObservacao.Remover();

            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExcluirNotificacaoObservacaoCartaIntencoes,
                       new ExcluirNotificacaoCartaIntencoesObservacaoDto(cartaIntencoesObservacao.Id), Guid.NewGuid(), null));

            return true;
        }
    }
}
