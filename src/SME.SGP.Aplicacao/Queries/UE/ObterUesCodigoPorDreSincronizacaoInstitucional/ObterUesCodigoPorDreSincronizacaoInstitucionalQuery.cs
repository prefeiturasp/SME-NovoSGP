using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigoPorDreSincronizacaoInstitucionalQuery : IRequest<IEnumerable<string>>
    {
        public ObterUesCodigoPorDreSincronizacaoInstitucionalQuery(long dreCodigo)
        {
            DreCodigo = dreCodigo;
        }
        public long DreCodigo { get; set; }
    }
}
