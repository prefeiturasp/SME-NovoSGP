using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioCommandHandler : IRequestHandler<NotificarUsuarioCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioUsuario repositorioUsuario;

        public NotificarUsuarioCommandHandler(IRepositorioNotificacao repositorioNotificacao,
                                              IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<bool> Handle(NotificarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(request.UsuarioRf, string.Empty);

            var notificacao = new Notificacao()
            {
                Codigo = ObtemNovoCodigo(),
                Titulo = request.Titulo,
                Mensagem = request.Mensagem,
                DreId = request.DreCodigo,
                UeId = request.UeCodigo,
                TurmaId = request.TurmaCodigo,
                Ano = request.Ano,
                Categoria = request.Categoria,
                Tipo = request.Tipo,
                UsuarioId = usuario?.Id,
            };

            await repositorioNotificacao.SalvarAsync(notificacao);
            return true;
        }

        public long ObtemNovoCodigo()
        {
            return repositorioNotificacao.ObterUltimoCodigoPorAno(DateTime.Now.Year) + 1;
        }

    }
}
