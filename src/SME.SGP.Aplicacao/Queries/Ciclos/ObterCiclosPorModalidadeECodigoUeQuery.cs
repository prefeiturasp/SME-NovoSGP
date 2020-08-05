using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeQuery : IRequest<IEnumerable<RetornoCicloDto>>
    {
        public ObterCiclosPorModalidadeECodigoUeQuery(int modalidade, string codigoUe)
        {
            Modalidade = modalidade;
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }

        public int Modalidade { get; set; }
    }
}
