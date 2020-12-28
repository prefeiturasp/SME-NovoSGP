using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorAlunoPeriodoUseCase : AbstractUseCase, IObterRegistrosIndividuaisPorAlunoPeriodoUseCase
    {
        public ObterRegistrosIndividuaisPorAlunoPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistrosIndividuaisPeriodoDto> Executar(FiltroRegistroIndividualAlunoPeriodo filtro)
        {
            var registrosIndividuais = await mediator.Send(new ObterRegistrosIndividuaisPorAlunoPeriodoQuery(filtro.TurmaId, filtro.AlunoCodigo, filtro.ComponenteCurricularId, filtro.DataInicio, filtro.DataFim));

            return registrosIndividuais;
        }
    }
}
