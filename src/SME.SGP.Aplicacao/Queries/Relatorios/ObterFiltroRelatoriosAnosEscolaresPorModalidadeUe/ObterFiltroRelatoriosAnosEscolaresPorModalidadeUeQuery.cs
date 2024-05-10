using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQuery(string codigoUe, Modalidade modalidade)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
        }

        public string CodigoUe { get; }
        public Modalidade Modalidade { get; }
    }
}
