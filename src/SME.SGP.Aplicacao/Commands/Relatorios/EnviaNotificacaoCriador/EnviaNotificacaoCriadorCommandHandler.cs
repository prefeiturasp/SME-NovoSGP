using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviaNotificacaoCriadorCommandHandler : IRequestHandler<EnviaNotificacaoCriadorCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public EnviaNotificacaoCriadorCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public Task<bool> Handle(EnviaNotificacaoCriadorCommand request, CancellationToken cancellationToken)
        {

            var urlNotificacao = $"{request.UrlRedirecionamentoBase}api/v1/relatorios/download/{request.RelatorioCorrelacao.Codigo}"; 

            var notificacao = new Notificacao()
            {
                Ano = request.RelatorioCorrelacao.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                Mensagem = $"O Relatório {request.RelatorioCorrelacao.TipoRelatorio.GetAttribute<DisplayAttribute>().Name} está pronto para download. <br /> Clique <a>aqui<a href='{urlNotificacao}' />",
                Tipo = NotificacaoTipo.Relatorio,
                UsuarioId = request.RelatorioCorrelacao.UsuarioSolicitanteId
            };
            
            repositorioNotificacao.Salvar(notificacao);

            return Task.FromResult(true);
        }
    }
}
