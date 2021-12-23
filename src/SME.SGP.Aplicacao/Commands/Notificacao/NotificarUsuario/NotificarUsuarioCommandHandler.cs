using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioCommandHandler : IRequestHandler<NotificarUsuarioCommand, long>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IMediator mediator;

        public NotificarUsuarioCommandHandler(IRepositorioNotificacao repositorioNotificacao, IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(NotificarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioId = request.UsuarioId > 0 ?
                request.UsuarioId :
                await mediator.Send(new ObterOuAdicionarUsuarioIdCommand(request.UsuarioRf, request.NomeUsuario));

            var notificacao = new Notificacao()
            {
                Codigo = request.Codigo == 0 ? await mediator.Send(new ObterNotificacaoUltimoCodigoPorAnoQuery(DateTime.Now.Year)) + 1 : request.Codigo,
                Titulo = request.Titulo,
                Mensagem = request.Mensagem,
                DreId = request.DreCodigo,
                UeId = request.UeCodigo,
                TurmaId = request.TurmaCodigo,
                Ano = request.Ano,
                Categoria = request.Categoria,
                Tipo = request.Tipo,
                UsuarioId = usuarioId,
            };

            if (request.CriadoEm.HasValue)
                notificacao.CriadoEm = request.CriadoEm.Value;

            return await repositorioNotificacao.SalvarAsync(notificacao);            
        }         
    }
}
