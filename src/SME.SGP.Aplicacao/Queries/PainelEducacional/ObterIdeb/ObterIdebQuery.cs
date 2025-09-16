using MediatR;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb
{
    public class ObterIdebQuery: IRequest<IEnumerable<PainelEducacionalIdeb>>
    {
    }
}
