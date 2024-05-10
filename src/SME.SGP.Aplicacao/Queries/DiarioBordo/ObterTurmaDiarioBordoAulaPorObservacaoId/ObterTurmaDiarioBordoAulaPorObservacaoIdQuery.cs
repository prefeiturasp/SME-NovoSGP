using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDiarioBordoAulaPorObservacaoIdQuery : IRequest<Turma>
    {
        public long ObservacaoId { get; set; }

        public ObterTurmaDiarioBordoAulaPorObservacaoIdQuery(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }
    }

    public class ObterTurmaDiarioBordoAulaPorObservacaoIdQueryValidator : AbstractValidator<ObterTurmaDiarioBordoAulaPorObservacaoIdQuery>
    {
        public ObterTurmaDiarioBordoAulaPorObservacaoIdQueryValidator()
        {
            RuleFor(x => x.ObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada para a busca da turma.");
        }
    }
}