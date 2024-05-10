using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoBimestreTurmaQuery : IRequest<(int bimestre, bool possuiConselho)>
    {
        public ObterUltimoBimestreTurmaQuery(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; set; }
    }

    public class ObterUltimoBimestreTurmaQueryValidator : AbstractValidator<ObterUltimoBimestreTurmaQuery>
    {
        public ObterUltimoBimestreTurmaQueryValidator()
        {
            RuleFor(c => c.Turma)
               .NotNull()
               .WithMessage("A turma deve ser informada.");               
        }
    }
}
