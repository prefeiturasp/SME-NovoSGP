using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioMapeamentoEstudanteQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioMapeamentoEstudanteQuery(FiltroQuestoesQuestionarioMapeamentoEstudanteDto filtro)
        {
            QuestionarioId = filtro.QuestionarioId;
            MapeamentoEstudanteId = filtro.MapeamentoEstudanteId;
            CodigoAluno = filtro.CodigoAluno;
            TurmaId = filtro.TurmaId;
            Bimestre = filtro.Bimestre;
        }

        public long QuestionarioId { get; set; }
        public long? MapeamentoEstudanteId { get; set; }
        public string CodigoAluno { get; set; }
        public long? TurmaId { get; set; }
        public int? Bimestre { get; set; }
    }

    public class ObterQuestionarioMapeamentoEstudanteQueryValidator : AbstractValidator<ObterQuestionarioMapeamentoEstudanteQuery>
    {
        public ObterQuestionarioMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("Os Id do questionário deve ser informado para consulta das questões de mapeamento de estudante.");

            RuleFor(x => x.CodigoAluno)
           .NotEmpty().When(x => !x.MapeamentoEstudanteId.HasValue)
           .WithMessage("O campo CodigoAluno é obrigatório quando QuestionarioId estiver vazio para consulta das questões de mapeamento de estudante.");

            RuleFor(x => x.TurmaId)
                .NotEmpty().When(x => !x.MapeamentoEstudanteId.HasValue)
                .WithMessage("O campo TurmaId é obrigatório quando QuestionarioId estiver vazio para consulta das questões de mapeamento de estudante.");

            RuleFor(x => x.Bimestre)
                .NotEmpty().When(x => !x.MapeamentoEstudanteId.HasValue)
                .WithMessage("O campo Bimestre é obrigatório quando QuestionarioId estiver vazio para consulta das questões de mapeamento de estudante.");

            RuleFor(x => x.MapeamentoEstudanteId)
                .NotEmpty().When(x => string.IsNullOrEmpty(x.CodigoAluno) && !x.TurmaId.HasValue && !x.Bimestre.HasValue)
                .WithMessage("O campo MapeamentoEstudanteId é obrigatório quando CodigoAluno, TurmaId e Bimestre estiverem vazios para consulta das questões de mapeamento de estudante.");
        }
    }
}
