using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaAlunoCommand : IRequest<bool>
    {
        public ExcluirItineranciaAlunoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaAlunoCommandValidator : AbstractValidator<ExcluirItineranciaAlunoCommand>
    {
        public ExcluirItineranciaAlunoCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id da itinerância do aluno deve ser informado para exclusão.");
        }
    }
}
