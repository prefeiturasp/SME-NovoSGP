using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase : AbstractUseCase, IExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase
    {
        public ExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(long encaminhamentoSecaoNAAPAId) => await mediator.Send(new ExcluirSecaoEncaminhamentoNAAPACommand(encaminhamentoSecaoNAAPAId));
    }

     
}
