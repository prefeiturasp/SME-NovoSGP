using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPreDefinidasUseCase : AbstractUseCase, IObterFrequenciasPreDefinidasUseCase
    {
        public ObterFrequenciasPreDefinidasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<FrequenciaPreDefinidaDto>> Executar(FiltroFrequenciaPreDefinidaDto filtro)
        {
            return await mediator.Send(new ObterFrequenciasPreDefinidasPorAlunoETurmaQuery(filtro.TurmaId, filtro.ComponenteCurricularId, filtro.CodigoAluno));
        }
    }
}
