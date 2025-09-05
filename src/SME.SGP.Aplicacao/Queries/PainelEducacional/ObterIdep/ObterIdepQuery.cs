using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep
{
    public class ObterIdepQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoIdep>>
    {
    }
}
