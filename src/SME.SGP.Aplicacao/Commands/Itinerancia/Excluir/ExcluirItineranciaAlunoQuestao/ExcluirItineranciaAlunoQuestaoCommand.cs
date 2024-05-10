using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaAlunoQuestaoCommand : IRequest<bool>
    {
        public ExcluirItineranciaAlunoQuestaoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaAlunoQuestaoCommandValidator : AbstractValidator<ExcluirItineranciaAlunoQuestaoCommand>
    {
        public ExcluirItineranciaAlunoQuestaoCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id da questão da itinerância do aluno deve ser informado para exclusão.");
        }
    }
}
