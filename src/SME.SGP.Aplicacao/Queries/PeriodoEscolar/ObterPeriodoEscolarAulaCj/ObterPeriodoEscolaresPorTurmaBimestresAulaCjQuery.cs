using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery : IRequest<PeriodoEscolarBimestreDto>
    {
        public ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery(Turma turma, int bimestre, bool aulaCj)
        {
            Turma = turma;
            Bimestre = bimestre;
            AulaCj = aulaCj;
        }

        public Turma Turma { get; set; }
        public int Bimestre { get; set; }
        public bool AulaCj { get; set; }
    }
    public class ObterPeriodoEscolaresPorTurmaBimestresAulaCjQueryValidator : AbstractValidator<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery>
    {
        public ObterPeriodoEscolaresPorTurmaBimestresAulaCjQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada para consulta do periodo escolar.");

            RuleFor(c => c.Bimestre)
               .InclusiveBetween(0, 4)
               .WithMessage("O bimestre deve ser informado para consulta do periodo escolar.");
        }
    }
}
