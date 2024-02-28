using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosTurmaQuery : IRequest<IEnumerable<ParecerConclusivoDto>>
    {
        public ObterPareceresConclusivosTurmaQuery(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; set; }
    }

    public class ObterPareceresConclusivosTurmaQueryValidator : AbstractValidator<ObterPareceresConclusivosTurmaQuery>
    {
        public ObterPareceresConclusivosTurmaQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informado para obter pareceres conclusivos");
        }
    }
}
