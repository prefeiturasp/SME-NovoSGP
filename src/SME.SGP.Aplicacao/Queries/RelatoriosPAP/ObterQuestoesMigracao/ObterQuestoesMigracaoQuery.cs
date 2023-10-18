using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesMigracaoQuery : IRequest<IEnumerable<Questao>>
    {
    }
}
