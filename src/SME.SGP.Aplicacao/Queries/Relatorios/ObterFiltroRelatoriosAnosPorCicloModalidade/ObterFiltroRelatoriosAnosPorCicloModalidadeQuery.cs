using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosPorCicloModalidadeQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterFiltroRelatoriosAnosPorCicloModalidadeQuery(long cicloId, Modalidade modalidade)
        {
            CicloId = cicloId;
            Modalidade = modalidade;            
        }

        public long CicloId { get; set; }
        public Modalidade Modalidade { get; set; }        
    }
}
