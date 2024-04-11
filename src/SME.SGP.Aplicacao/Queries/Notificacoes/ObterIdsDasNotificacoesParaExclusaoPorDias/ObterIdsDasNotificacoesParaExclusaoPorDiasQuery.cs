using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDasNotificacoesParaExclusaoPorDiasQuery : IRequest<IEnumerable<long>>
    {
    }
}
