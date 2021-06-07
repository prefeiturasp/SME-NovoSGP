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
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
        }
        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Handle(ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            int anoTurma;
            var listaRetorno = new List<ObjetivoAprendizagemDto>();

            foreach (var ano in request.Anos)
            {
                listaRetorno.AddRange(await repositorioObjetivoAprendizagem
                    .ObterPorAnoEComponenteCurricularJuremaIds(int.TryParse(ano, out anoTurma) ? (AnoTurma)Convert.ToInt32(ano) : (AnoTurma?)null, request.JuremaIds));
            }

            return listaRetorno;
        }
    }
}
