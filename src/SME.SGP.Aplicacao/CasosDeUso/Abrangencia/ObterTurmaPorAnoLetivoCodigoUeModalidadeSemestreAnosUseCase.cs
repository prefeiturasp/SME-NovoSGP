using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Abrangencia
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase : AbstractUseCase, IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, IList<string> anos)
        {
            return await mediator.Send(new ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery(anoLetivo, codigoUe, modalidade, semestre, anos));
        }
    }
}
