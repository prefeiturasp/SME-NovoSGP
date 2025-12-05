using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioAtendimentoNAAPADtoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {

        public ObterSecoesQuestionarioAtendimentoNAAPADtoQuery(int modalidade)
        {
            Modalidade = modalidade;
        }

        public int Modalidade { get; }
    }

    public class ObterSecoesQuestionarioAtendimentoNAAPADtoQueryValidator : AbstractValidator<ObterSecoesQuestionarioAtendimentoNAAPADtoQuery>
    {
        public ObterSecoesQuestionarioAtendimentoNAAPADtoQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para obter as seções de questionário do atendimento NAAPA.");
        }
    }
}
