using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPendenciasFechamentoCommandHandler : IRequestHandler<ExcluirNotificacaoPendenciasFechamentoCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ExcluirNotificacaoPendenciasFechamentoCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<bool> Handle(ExcluirNotificacaoPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = await this.repositorioNotificacao.ObterIdsAsync(request.TurmaCodigo, NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, request.Ano);
            if (notificacoes.Any())
            {
                await this.repositorioNotificacao.ExcluirLogicamentePorIdsAsync(notificacoes);
                return true;
            }
            return false;
        }

    }
}
