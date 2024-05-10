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
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler : IRequestHandler<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery, IEnumerable<ObjetivoAprendizagemDto>>
    {
        private readonly IRepositorioObjetivoAprendizagemConsulta repositorioObjetivoAprendizagem;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQueryHandler(IRepositorioObjetivoAprendizagemConsulta repositorioObjetivoAprendizagem)
        {
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
        }
        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Handle(ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var listaRetorno = new List<ObjetivoAprendizagemDto>();

            if (request.Anos.EhNulo() || !request.Anos.Any())
            {
                listaRetorno.AddRange(
                    await repositorioObjetivoAprendizagem
                        .ObterPorAnoEComponenteCurricularJuremaIds(null, request.JuremaIds));
            }
            else
            {
                foreach (var ano in request.Anos)
                {
                    var anoTurmaEInteiro = int.TryParse(ano, out int anoTurma);

                    listaRetorno.AddRange(
                        await repositorioObjetivoAprendizagem
                            .ObterPorAnoEComponenteCurricularJuremaIds(anoTurmaEInteiro ? (AnoTurma)anoTurma : null, request.JuremaIds));
                }
            }

            return listaRetorno;
        }
    }
}
