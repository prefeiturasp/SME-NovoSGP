using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaAlunoQuery : IRequest<IEnumerable<ItineranciaAlunoQuestaoDto>>
    {
        public ObterQuestoesItineranciaAlunoQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterQuestoesItineranciaAlunoQueryValidator : AbstractValidator<ObterQuestoesItineranciaAlunoQuery>
    {
        public ObterQuestoesItineranciaAlunoQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerancia do aluno deve ser informado.");

        }
    }
}
