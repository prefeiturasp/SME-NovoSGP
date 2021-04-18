using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeQuery : IRequest<IEnumerable<RetornoCicloDto>>
    {
        public ObterCiclosPorModalidadeECodigoUeQuery(int modalidade, string codigoUe, bool consideraAbrangencia)
        {
            Modalidade = modalidade;
            CodigoUe = codigoUe;
            ConsideraAbrangencia = consideraAbrangencia;
        }

        public string CodigoUe { get; set; }

        public int Modalidade { get; set; }
        public bool ConsideraAbrangencia { get; set; }

    }
}
