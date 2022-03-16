using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
            foreach (var usuario in request.Usuarios)
            {
                var notificacao = new Notificacao()
                {
                    Codigo = await mediator.Send(new ObterNotificacaoUltimoCodigoPorAnoQuery(DateTime.Now.Year)) + 1,
                    Ano = DateTime.Today.Year,
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
            }

            return notificacoes;
        }
    }
}
