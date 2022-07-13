using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarNotificacaoCommandHandler : IRequestHandler<GerarNotificacaoCommand, bool>
    {
        private readonly IMediator mediator;

        private readonly IServicoNotificacao servicoNotificacao;

        public GerarNotificacaoCommandHandler(IMediator mediator,IServicoNotificacao servicoNotificacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<bool> Handle(GerarNotificacaoCommand request, CancellationToken cancellationToken)
        {
            servicoNotificacao.Salvar(new Notificacao()
            {
                Ano = request.Ano,
                Categoria = request.Categoria,
                DreId = request.DreId,
                Mensagem = request.Mensagem,
                UsuarioId = request.UsuarioId,
                Tipo = request.Tipo,
                Titulo =request.Titulo,
                TurmaId = request.TurmaId,
                UeId = request.UeId,
            });

            return true;
        }
    }
}
