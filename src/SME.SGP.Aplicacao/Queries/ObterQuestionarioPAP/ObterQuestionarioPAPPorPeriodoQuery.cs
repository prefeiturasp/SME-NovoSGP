using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPPorPeriodoQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioPAPPorPeriodoQuery(string codigoTurma, string codigoAluno, long periodoIdPap, long questionarioId, long? papSecaoId)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            PeriodoIdPAP = periodoIdPap;
            QuestionarioId = questionarioId;
            PapSecaoId = papSecaoId;
        }

        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public long PeriodoIdPAP { get; set; }
        public long QuestionarioId { get; set; }
        public long? PapSecaoId { get; set; }
    }

    public class ObterQuestionarioPAPPorPeriodoQueryValidator : AbstractValidator<ObterQuestionarioPAPPorPeriodoQuery>
    {
        public ObterQuestionarioPAPPorPeriodoQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("Informe o código da turma");
            RuleFor(x => x.CodigoAluno).NotEmpty().WithMessage("Informe o código do aluno ");
            RuleFor(x => x.PeriodoIdPAP).GreaterThan(0).WithMessage("Informe o período pap");
            RuleFor(x => x.QuestionarioId).GreaterThan(0).WithMessage("Informe o Questionario");
        }
    }
}