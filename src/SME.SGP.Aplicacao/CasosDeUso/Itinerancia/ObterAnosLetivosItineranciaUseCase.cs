using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosItineranciaUseCase : AbstractUseCase, IObterAnosLetivosItineranciaUseCase
    {
        public ObterAnosLetivosItineranciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<int>> Executar()
        {
            var anosLetivosDasItinerancias = await mediator.Send(new ObterAnosLetivosItineranciaQuery());

            if (anosLetivosDasItinerancias.Count() == 1 && anosLetivosDasItinerancias.FirstOrDefault() == DateTime.Now.Year)
                return anosLetivosDasItinerancias;

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var anosLetivosAbrangenciaHistorico = await mediator.Send(new ObterUsuarioAbrangenciaAnosLetivosQuery(usuarioLogado.Login, true, usuarioLogado.PerfilAtual, 0));

            var anosParaRetorno = new HashSet<int>();

            anosParaRetorno.UnionWith(anosLetivosDasItinerancias);

            var anosAnterioresParaIncluir = anosLetivosAbrangenciaHistorico.Where(a => anosLetivosDasItinerancias.Contains(a));

            anosParaRetorno.UnionWith(anosAnterioresParaIncluir);

            return anosParaRetorno.OrderBy(a => a).AsEnumerable();


        }
                
    }
}
