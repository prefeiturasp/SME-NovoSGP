using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoDeFechamentoQuery : IRequest<bool>
    {
        public Turma Turma { get; set; }
        public DateTime Data {  get; set; }
        public int Bimestre { get; set; }

        public TurmaEmPeriodoDeFechamentoQuery(Turma turma, DateTime data, int bimestre)
        {
            Turma = turma;
            Data = data;
            Bimestre = bimestre;
        }
    }

    public class TurmaEmPeriodoDeFechamentoQueryValidator : AbstractValidator<TurmaEmPeriodoDeFechamentoQuery>
    {
        public TurmaEmPeriodoDeFechamentoQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada para consulta de existencia de periodo de fechamento.");

            RuleFor(c => c.Data)
               .NotEmpty()
               .WithMessage("A data deve ser informada para consulta de existencia de periodo de fechamento.");

            RuleFor(c => c.Bimestre)
               .NotEmpty()
               .WithMessage("O bimestre deve ser informada para consulta de existencia de periodo de fechamento.");
        }
    }
}
