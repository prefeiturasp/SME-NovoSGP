using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTurmaBimestreQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarPorTurmaBimestreQuery(Turma turma, int bimestre)
        {
            Turma = turma;
            Bimestre = bimestre;
        }

        public Turma Turma { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterPeriodoEscolarPorTurmaBimestreQueryValidator : AbstractValidator<ObterPeriodoEscolarPorTurmaBimestreQuery>
    {
        public ObterPeriodoEscolarPorTurmaBimestreQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada para consulta do periodo escolar.");

            RuleFor(c => c.Bimestre)
               .InclusiveBetween(0,4)
               .WithMessage("O bimestre deve ser informado para consulta do periodo escolar.");
        }
    }
}