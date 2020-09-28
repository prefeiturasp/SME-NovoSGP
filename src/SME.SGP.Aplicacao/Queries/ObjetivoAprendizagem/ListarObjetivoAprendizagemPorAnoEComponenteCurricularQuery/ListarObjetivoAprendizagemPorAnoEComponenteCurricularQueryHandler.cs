using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler : IRequestHandler<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery, IEnumerable<ObjetivoAprendizagem>>
    {
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler(IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem)
        {
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem;
        }
        public async Task<IEnumerable<ObjetivoAprendizagem>> Handle(ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return await repositorioObjetivoAprendizagem.ObterPorAnoEComponenteCurricularIdAsync((AnoTurma)request.Ano, request.ComponenteCurricularId);
        }
    }
}
