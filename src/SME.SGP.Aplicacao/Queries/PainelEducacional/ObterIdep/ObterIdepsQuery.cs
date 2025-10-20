using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional
{
    public class ObterIdepsQuery : IRequest<IEnumerable<Idep>>
    {
        public int AnoLetivo { get; }
        public string CodigoEOLEscola { get; }

        public ObterIdepsQuery(int anoLetivo, string codigoEOLEscola)
        {
            AnoLetivo = anoLetivo;
            CodigoEOLEscola = codigoEOLEscola;
        }
    }
}