using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioMapeamentoEstudanteDtoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {

        public ObterSecoesQuestionarioMapeamentoEstudanteDtoQuery()
        { }

    }

}
