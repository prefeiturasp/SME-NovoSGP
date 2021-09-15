using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery: IRequest<PeriodoFechamentoBimestre>
    {
        public Turma Turma { get; set; }
        public int Bimestre { get; set; }
        public long PeriodoEscolarId { get; set; }

        public ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery(Turma turma, int bimestre, long periodoEscolarId)
        {
            Turma = turma;
            Bimestre = bimestre;
            PeriodoEscolarId = periodoEscolarId;
        }
    }

    public class ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQueryValidator : AbstractValidator<ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery>
    {
        public ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Bimestre)
               .Must(a => a > 0)
               .WithMessage("O bimestre do fechamento deve ser informado.");

            RuleFor(c => c.PeriodoEscolarId)
               .Must(a => a > 0)
               .WithMessage("O período escolar do fechamento deve ser informado.");
        }
    }
}
