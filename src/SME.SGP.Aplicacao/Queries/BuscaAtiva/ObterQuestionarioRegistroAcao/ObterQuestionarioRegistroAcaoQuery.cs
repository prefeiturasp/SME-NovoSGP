using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioRegistroAcaoQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioRegistroAcaoQuery(long questionarioId, long? registroAcaoId)
        {
            QuestionarioId = questionarioId;
            RegistroAcaoId = registroAcaoId;
        }

        public long QuestionarioId { get; }
        public long? RegistroAcaoId { get; }
    }

    public class ObterQuestionarioRegistroAcaoQueryValidator : AbstractValidator<ObterQuestionarioRegistroAcaoQuery>
    {
        public ObterQuestionarioRegistroAcaoQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de registro de ação.");
        }
    }
}
