using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb
{
    public class PainelEducacionalIdebQuery : IRequest<IEnumerable<PainelEducacionalIdebDto>>
    {
    }
}
