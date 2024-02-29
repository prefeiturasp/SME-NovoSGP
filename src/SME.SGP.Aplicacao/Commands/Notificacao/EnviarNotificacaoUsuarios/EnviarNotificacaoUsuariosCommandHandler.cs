using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoUsuariosCommandHandler : IRequestHandler<EnviarNotificacaoUsuariosCommand, IEnumerable<long>>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IMediator mediator;

        public EnviarNotificacaoUsuariosCommandHandler(IRepositorioNotificacao repositorioNotificacao, IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<long>> Handle(EnviarNotificacaoUsuariosCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = new List<long>();
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            foreach (var usuario in request.Usuarios)
            {
                var notificacao = new Notificacao()
                {
                    Codigo = await mediator.Send(new ObterNotificacaoUltimoCodigoPorAnoQuery(anoAtual)) + 1,
                    Ano = anoAtual,
                    Categoria = request.CategoriaNotificacao,
                    Tipo = request.TipoNotificacao,
                    DreId = request.DreCodigo,
                    UeId = request.UeCodigo,
                    TurmaId = request.TurmaCodigo,
                    Titulo = request.Titulo,
                    Mensagem = request.Mensagem,
                    UsuarioId = usuario
                };

                notificacoes.Add(await repositorioNotificacao.SalvarAsync(notificacao));

                await mediator.Send(new NotificarCriacaoNotificacaoCommand(notificacao));
            }

            return notificacoes;
        }
    }
}
