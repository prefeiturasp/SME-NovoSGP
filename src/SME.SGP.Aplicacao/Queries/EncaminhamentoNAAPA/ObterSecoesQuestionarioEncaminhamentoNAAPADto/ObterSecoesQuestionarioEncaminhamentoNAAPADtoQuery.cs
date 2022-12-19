using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery()
        {}

    }
}
