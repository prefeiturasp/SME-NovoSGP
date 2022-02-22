using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigosPorModalidadeEAnoLetivoQuery : IRequest<IEnumerable<string>>
    {
        public ObterUesCodigosPorModalidadeEAnoLetivoQuery(Modalidade modalidade, int anoLetivo, int pagina = 1)
        {
            Modalidade = modalidade;
            AnoLetivo = anoLetivo;
            Pagina = pagina;
        }

        public Modalidade Modalidade { get; set; }
        public int AnoLetivo { get; set; }
        public int Pagina { get; set; }
    }
}
