using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaAlunoPorIdQuery : IRequest<IEnumerable<ItineranciaAlunoQuestaoDto>>
    {
        public ObterQuestoesItineranciaAlunoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterQuestoesItineranciaAlunoPorIdQueryValidator : AbstractValidator<ObterQuestoesItineranciaAlunoPorIdQuery>
    {
        public ObterQuestoesItineranciaAlunoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerância do aluno deve ser informado.");

        }
    }
}
