using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolaresPorTurmaBimestresQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolaresPorTurmaBimestresQuery(Turma turma, int[] bimestres)
        {
            Turma = turma;
            Bimestres = bimestres;
        }

        public Turma Turma { get; set; }
        public int[] Bimestres { get; set; }
    }

    public class ObterPeriodoEscolaresPorTurmaBimestresQueryValidator : AbstractValidator<ObterPeriodoEscolaresPorTurmaBimestresQuery>
    {
        public ObterPeriodoEscolaresPorTurmaBimestresQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada para consulta do periodo escolar.");

            RuleFor(c => c.Bimestres)
               .NotNull()
               .NotEmpty()
               .WithMessage("Os bimestres devem ser informados para consulta do periodo escolar.");
        }
    }
}
