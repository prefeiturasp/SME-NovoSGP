using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosPorTurmaQuery : IRequest<IEnumerable<ConselhoClasseParecerConclusivo>>
    {
        public ObterPareceresConclusivosPorTurmaQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; }
    }

    public class ObterPareceresConclusivosPorTurmaQueryValidator : AbstractValidator<ObterPareceresConclusivosPorTurmaQuery>
    {
        public ObterPareceresConclusivosPorTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para consultar seus pareceres conclusivos");
        }
    }
}
