using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadeAvaliativaPorGoogleClassroomIdQuery : IRequest<AtividadeAvaliativa>
    {
        public long GoogleClassroomId { get; set; }

        public ObterAtividadeAvaliativaPorGoogleClassroomIdQuery(long googleClassroomId)
        {
            GoogleClassroomId = googleClassroomId;
        }
    }

    public class
        ObterNotaPorGoogleClassroomIdQueryValidator : AbstractValidator<
            ObterAtividadeAvaliativaPorGoogleClassroomIdQuery>
    {
        public ObterNotaPorGoogleClassroomIdQueryValidator()
        {
            RuleFor(c => c.GoogleClassroomId)
                .NotEmpty()
                .WithMessage("A Google Classroom id deve ser informada.");
        }
    }
}