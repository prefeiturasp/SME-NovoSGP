using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioItineranciasUseCase : AbstractUseCase, IRelatorioItineranciasUseCase
    {
        public RelatorioItineranciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(IEnumerable<long> itinerancias)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var filtro = new FiltroRelatorioItineranciaDto()
            {
                Itinerancias = itinerancias,
                UsuarioNome = usuarioLogado.Nome,
                UsuarioRF = usuarioLogado.CodigoRf
            };

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Itinerancias, filtro, usuarioLogado));
        }
    }
}
