using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaAlunoCommand : IRequest<bool>
    {
        public ExcluirItineranciaAlunoCommand(ItineranciaAluno aluno)
        {
            Aluno = aluno;
        }

        public ItineranciaAluno Aluno { get; set; }
    }
    public class ExcluirItineranciaAlunoCommandValidator : AbstractValidator<ExcluirItineranciaAlunoCommand>
    {
        public ExcluirItineranciaAlunoCommandValidator()
        {
            RuleFor(c => c.Aluno)
            .NotEmpty()
            .WithMessage("O id da itinerância do aluno deve ser informado para exclusão.");
        }
    }
}
