using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigosPorModalidadeEAnoLetivoQuery : IRequest<IEnumerable<string>>
    {
        public ObterUesCodigosPorModalidadeEAnoLetivoQuery(Modalidade modalidade, int anoLetivo)
        {
            Modalidade = modalidade;
            AnoLetivo = anoLetivo;
        }

        public Modalidade Modalidade { get; set; }
        public int AnoLetivo { get; set; }
    }
}
