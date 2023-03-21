using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAjudasDoSistemaQuery : IRequest<IEnumerable<AjudaDoSistemaDto>>
    {
    }
}