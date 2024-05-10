using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioEncaminhamentoNAAPAQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioEncaminhamentoNAAPAQuery(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma)
        {
            QuestionarioId = questionarioId;
            EncaminhamentoId = encaminhamentoId;
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
        }

        public long QuestionarioId { get; }
        public long? EncaminhamentoId { get; }
        public string CodigoAluno { get; }
        public string CodigoTurma { get; }
    }

    public class ObterQuestionarioEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterQuestionarioEncaminhamentoNAAPAQuery>
    {
        public ObterQuestionarioEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de encaminhamento NAAPA.");

            RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado para consulta do questionário de encaminhamento NAAPA.");

            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma do aluno deve ser informado para consulta do questionário de encaminhamento NAAPA.");
        }
    }
}
