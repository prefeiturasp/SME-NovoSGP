using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {

        public ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery(int modalidade)
        {
            Modalidade = modalidade;
        }

        public int Modalidade { get; }
    }

    public class ObterSecoesQuestionarioEncaminhamentoNAAPADtoQueryValidator : AbstractValidator<ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery>
    {
        public ObterSecoesQuestionarioEncaminhamentoNAAPADtoQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para obter as seções de questionário do encaminhamento NAAPA.");
        }
    }
}
