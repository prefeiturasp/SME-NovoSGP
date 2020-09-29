using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler : AbstractUseCase, IRequestHandler<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery, IEnumerable<ObjetivoAprendizagemDto>>
    {
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler(IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem, IMediator mediator) : base(mediator)
        {
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem;
        }
        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Handle(ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            long[] ids;

            if (request.ComponenteCurricularId == 138)
            {
                ids = new long[] { (request.EnsinoEspecial ? 11 : 6) };
            }
            else
            {
                ids = await mediator.Send(new ObterJuremaIdsPorComponentesCurricularIdQuery(request.ComponenteCurricularId));
            }

            var objetivos = await repositorioObjetivoAprendizagem.ObterPorAnoEComponenteCurricularJuremaIds((AnoTurma)request.Ano, ids);

            foreach (var item in objetivos)
            {
                item.ComponenteCurricularEolId = request.ComponenteCurricularId;
            }

            IEnumerable<int> anos = Enumerable.Range(1, 9);
            if (request.EnsinoEspecial && !anos.Select(a => a.ToString()).Contains(request.Ano.ToString()))
            {
                return objetivos.OrderBy(o => o.Ano).ThenBy(x => x.Codigo);
            }

            objetivos = objetivos.Where(x => x.Ano == ((AnoTurma)request.Ano).Name()).ToList();
            return objetivos.OrderBy(o => o.Codigo);
        }
    }
}
