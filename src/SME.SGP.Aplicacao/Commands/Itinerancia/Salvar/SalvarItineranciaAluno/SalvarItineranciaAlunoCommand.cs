using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaAlunoCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaAlunoCommand(ItineranciaAlunoDto aluno, long itineranciaId)
        {
            Aluno = aluno;
            ItineranciaId = itineranciaId;
        }

        public ItineranciaAlunoDto Aluno { get; set; }
        public long ItineranciaId { get; set; }

    }

    public class SalvarItineranciaAlunoCommandValidator : AbstractValidator<SalvarItineranciaAlunoCommand>
    {
        public SalvarItineranciaAlunoCommandValidator()
        {
            RuleFor(x => x.Aluno)
                   .NotEmpty()
                   .WithMessage("O Aluno da itinerância deve ser informado!");
            RuleFor(x => x.ItineranciaId)
                   .GreaterThan(0)
                   .WithMessage("O id da itinerância deve ser informado!");
        }
    }
}
