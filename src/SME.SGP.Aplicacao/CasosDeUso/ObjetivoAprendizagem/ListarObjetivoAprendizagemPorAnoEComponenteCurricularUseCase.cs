using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
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

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(string ano, long componenteCurricularId, bool ensinoEspecial)
        {
            long[] ids;

            if (componenteCurricularId == 138)
                ids = new long[] { (ensinoEspecial ? 11 : 6) };
            else
                ids = await mediator.Send(new ObterJuremaIdsPorComponentesCurricularIdQuery(componenteCurricularId));

            IEnumerable<int> anosFundamental = Enumerable.Range(1, 9);

            var objetivos = await mediator.Send(
                new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(ensinoEspecial ? anosFundamental.Select(a => a.ToString()).ToArray() : new string[] { ano }, ids));

            foreach (var item in objetivos)
                item.ComponenteCurricularEolId = componenteCurricularId;

            if (ensinoEspecial && !anosFundamental.Select(a => a.ToString()).Contains(ano.ToString()))
                return objetivos.OrderBy(o => Enum.Parse(typeof(AnoTurma), o.Ano)).ThenBy(x => x.Codigo);

            var anoTurma = Convert.ToInt32(ano);

            objetivos = objetivos.Where(x => x.Ano == ((AnoTurma)anoTurma).Name()).ToList();
            return objetivos.OrderBy(o => o.Codigo);
        }
    }
}
