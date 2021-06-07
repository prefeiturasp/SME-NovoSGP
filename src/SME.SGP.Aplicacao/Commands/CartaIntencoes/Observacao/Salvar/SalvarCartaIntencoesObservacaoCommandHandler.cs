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
    public class SalvarCartaIntencoesObservacaoCommandHandler : IRequestHandler<SalvarCartaIntencoesObservacaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IMediator mediator;

        public SalvarCartaIntencoesObservacaoCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao, IRepositorioTurma repositorioTurma,
            IMediator mediator)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(SalvarCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = new CartaIntencoesObservacao(request.Observacao, request.TurmaId, request.ComponenteCurricularId, request.UsuarioId); ;
            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(request.TurmaId);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNovaNotificacaoObservacaoCartaIntencoes,
                       new SalvarNotificacaoCartaIntencoesObservacaoDto(turma, usuarioLogado, cartaIntencoesObservacao.Id, request.Observacao), Guid.NewGuid(), null));


            return (AuditoriaDto)cartaIntencoesObservacao;
        }
    }
}
