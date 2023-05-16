using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery : IRequest<string[]>
    {
        public ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery(long[] codigosComponentesTerritorio, string codigoTurma, string professorDesconsiderado)
        {
            CodigosComponentesTerritorio = codigosComponentesTerritorio;
            CodigoTurma = codigoTurma;
            ProfessorDesconsiderado = professorDesconsiderado;
        }

        public long[] CodigosComponentesTerritorio { get; set; }
        public string CodigoTurma { get; set; }
        public string ProfessorDesconsiderado { get; set; }
    }

    public class ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQueryValidator : AbstractValidator<ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery>
    {
        public ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQueryValidator()
        {
            RuleFor(x => x.CodigosComponentesTerritorio)
                .NotEmpty()
                .NotNull()
                .WithMessage("É necessário informar os códigos de componentes de território.");

            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .NotNull()
                .WithMessage("É necessário o código da turma.");

            RuleFor(x => x.ProfessorDesconsiderado)
                .NotEmpty()
                .NotNull()
                .WithMessage("É necessário o código da turma.");
        }
    }
}
