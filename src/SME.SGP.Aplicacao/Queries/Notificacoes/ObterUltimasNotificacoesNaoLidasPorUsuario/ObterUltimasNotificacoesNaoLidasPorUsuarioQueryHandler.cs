using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasNotificacoesNaoLidasPorUsuarioQueryHandler : IRequestHandler<ObterUltimasNotificacoesNaoLidasPorUsuarioQuery, IEnumerable<NotificacaoBasicaDto>>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ObterUltimasNotificacoesNaoLidasPorUsuarioQueryHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }
        public async Task<IEnumerable<NotificacaoBasicaDto>> Handle(ObterUltimasNotificacoesNaoLidasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var listaTratada = await repositorioNotificacao.ObterNotificacoesPorAnoLetivoERfAsync(request.AnoLetivo, request.CodigoRf, 5);

            if (request.TituloReduzido)
            {
                var listaRetorno = new List<NotificacaoBasicaDto>();

                foreach (var item in listaTratada)
                {
                    var notificao = (NotificacaoBasicaDto)item.Clone();
                    notificao.Titulo = notificao.ObtemTituloRudizoParaCaixaNotificacao();
                    listaRetorno.Add(notificao);
                }
                return await Task.FromResult(listaRetorno);
            }
            else return await Task.FromResult(listaTratada);


        }
    }
}
