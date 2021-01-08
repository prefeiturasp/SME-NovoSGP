using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioEncaminhamentoAeeQuery : IRequest<IEnumerable<QuestaoAeeDto>>
    {
        public ObterQuestionarioEncaminhamentoAeeQuery(long questionarioId, long? encaminhamentoId)
        {
            QuestionarioId = questionarioId;
            EncaminhamentoId = encaminhamentoId;
        }

        public long QuestionarioId { get; }
        public long? EncaminhamentoId { get; }
    }

    public class ObterQuestionarioEncaminhamentoAeeQueryValidator : AbstractValidator<ObterQuestionarioEncaminhamentoAeeQuery>
    {
        public ObterQuestionarioEncaminhamentoAeeQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado.");
        }
    }
}
