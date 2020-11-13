using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase : IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase
    {
        private readonly IMediator mediator;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(long ano, long componenteCurricularId, bool ensinoEspecial)
        {
            long[] ids;

            if (componenteCurricularId == 138)
            {
                ids = new long[] { (ensinoEspecial ? 11 : 6) };
            }
            else
            {
                ids = await mediator.Send(new ObterJuremaIdsPorComponentesCurricularIdQuery(componenteCurricularId));
            }

            var objetivos = await mediator.Send(new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(ano, ids));

            

            foreach (var item in objetivos)
            {
                item.ComponenteCurricularEolId = componenteCurricularId;
            }

            IEnumerable<int> anos = Enumerable.Range(1, 9);
            if (ensinoEspecial && !anos.Select(a => a.ToString()).Contains(ano.ToString()))
            {
                return objetivos.OrderBy(o => o.Ano).ThenBy(x => x.Codigo);
            }

            objetivos = objetivos.Where(x => x.Ano == ((AnoTurma)ano).Name()).ToList();
            return objetivos.OrderBy(o => o.Codigo);
        }
    }
}
