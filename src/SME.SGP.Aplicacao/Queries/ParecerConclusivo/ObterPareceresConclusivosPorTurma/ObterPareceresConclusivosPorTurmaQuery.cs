using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosPorTurmaQuery : IRequest<IEnumerable<ConselhoClasseParecerConclusivo>>
    {
        public ObterPareceresConclusivosPorTurmaQuery(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; }
    }

    public class ObterPareceresConclusivosPorTurmaQueryValidator : AbstractValidator<ObterPareceresConclusivosPorTurmaQuery>
    {
        public ObterPareceresConclusivosPorTurmaQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informado para consultar seus pareceres conclusivos");
        }
    }
}
