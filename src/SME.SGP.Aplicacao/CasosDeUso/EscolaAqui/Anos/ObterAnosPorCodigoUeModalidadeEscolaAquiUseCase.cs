using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase : AbstractUseCase, IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase
    {
        public ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Executar(string codigoUe, int[] modalidades)
        {
            if (modalidades.Any(m => (Modalidade)m != Modalidade.Fundamental &&
                                     (Modalidade)m != Modalidade.Medio &&
                                     (Modalidade)m != Modalidade.EJA))
            {
                return new List<AnosPorCodigoUeModalidadeEscolaAquiResult>()
                {
                    new AnosPorCodigoUeModalidadeEscolaAquiResult()
                    {
                        Ano = "-99",
                    }
                };
            }

            var anos = await mediator.Send(new ObterAnosPorCodigoUeModalidadeQuery(codigoUe, modalidades));
            return anos.Any() ? anos :
                new List<AnosPorCodigoUeModalidadeEscolaAquiResult>()
                {
                    new AnosPorCodigoUeModalidadeEscolaAquiResult() {
                        Ano = "-99"
                    }
                };
        }
    }
}
