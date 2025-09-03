using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ObterIdep
{
    public class ObterIdepCommand : IRequest<IEnumerable<PainelEducacionalIdep>>
    {
    }
}
