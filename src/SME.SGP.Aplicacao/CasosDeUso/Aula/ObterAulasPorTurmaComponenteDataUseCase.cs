using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorTurmaComponenteDataUseCase: AbstractUseCase, IObterAulasPorTurmaComponenteDataUseCase
    {
        public ObterAulasPorTurmaComponenteDataUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<IEnumerable<AulaQuantidadeTipoDto>> Executar(FiltroObterAulasPorTurmaComponenteDataDto filtro)
        {
            var aulas = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularQuery(filtro.DataAula,filtro.TurmaCodigo, filtro.ComponenteCurricular));
            
            return  aulas.Select(s=> new AulaQuantidadeTipoDto
            {
                Id = s.Id,
                Quantidade = s.Quantidade,
                Tipo = (int)s.TipoAula,
                EhCj = s.AulaCJ
            });
        }
    }
}
