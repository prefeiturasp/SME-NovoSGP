using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoQuery : IRequest<double>
    {
        public ObterNotaEmAprovacaoQuery(string codigoAluno, long turmafechamentoId, long disciplinaId)
        {
            CodigoAluno = codigoAluno;
            TurmaFechamentoId = turmafechamentoId;
            DisciplinaId = disciplinaId;
        }
        public string CodigoAluno { get; set; }
        public long TurmaFechamentoId { get; set; }
        public long DisciplinaId { get; set; }
        public long? PeriodoEscolarId { get; set; }
    }

    public class ObterNotaEmAprovacaoQueryValidator : AbstractValidator<ObterNotaEmAprovacaoQuery>
    {
        public ObterNotaEmAprovacaoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno");

            RuleFor(a => a.TurmaFechamentoId)
                .NotEmpty()
                .WithMessage("Necessário informar o Código da Turma Fechamento");

            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Código da Disciplina");
        }
    }
}
