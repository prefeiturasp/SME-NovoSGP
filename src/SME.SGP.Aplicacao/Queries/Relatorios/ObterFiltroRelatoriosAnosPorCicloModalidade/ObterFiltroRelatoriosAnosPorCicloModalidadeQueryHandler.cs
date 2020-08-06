using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosPorCicloModalidadeQueryHandler : IRequestHandler<ObterFiltroRelatoriosAnosPorCicloModalidadeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAnoEscolar repositorioAnoEscolar;

        public ObterFiltroRelatoriosAnosPorCicloModalidadeQueryHandler(IRepositorioAnoEscolar  repositorioAnoEscolar)
        {
            this.repositorioAnoEscolar = repositorioAnoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioAnoEscolar));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosAnosPorCicloModalidadeQuery request, CancellationToken cancellationToken)
        {
            var listaRetorno = await repositorioAnoEscolar.ObterPorModalidadeCicloId(request.Modalidade, request.CicloId);
            if (!listaRetorno.Any())
                throw new NegocioException("Não foi possível localizar Anos escolares.");

            return listaRetorno.Select(a => new OpcaoDropdownDto(a.Ano.ToString(), $"{a.Ano}º ano - {a.Modalidade.Name()} ") );
        }

    
    }
}
