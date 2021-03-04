using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoUsuariosCommandHandler : IRequestHandler<EnviarNotificacaoUsuariosCommand, long>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public EnviarNotificacaoUsuariosCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<long> Handle(EnviarNotificacaoUsuariosCommand request, CancellationToken cancellationToken)
        {
            Notificacao notificacao = null;
            foreach (var usuario in request.Usuarios)
            {
                notificacao = new Notificacao()
                {
                    Codigo = ObtemNovoCodigo(),
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

                await repositorioNotificacao.SalvarAsync(notificacao);
            }

            return notificacao.Id;
        }
        public long ObtemNovoCodigo()
        {
            return repositorioNotificacao.ObterUltimoCodigoPorAno(DateTime.Now.Year) + 1;
        }

    }
}
