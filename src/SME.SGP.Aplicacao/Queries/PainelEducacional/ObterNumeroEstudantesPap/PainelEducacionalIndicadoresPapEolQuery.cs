using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroEstudantesPap
{
    public class PainelEducacionalIndicadoresPapEolQuery : IRequest<IEnumerable<ContagemNumeroAlunosPapDto>>
    {
    }
}