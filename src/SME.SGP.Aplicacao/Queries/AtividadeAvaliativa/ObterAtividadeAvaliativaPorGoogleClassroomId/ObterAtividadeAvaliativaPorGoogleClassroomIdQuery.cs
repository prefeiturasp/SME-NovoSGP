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
        ObterAtividadeAvaliativaPorGoogleClassroomIdQueryValidator : AbstractValidator<
            ObterAtividadeAvaliativaPorGoogleClassroomIdQuery>
    {
        public ObterAtividadeAvaliativaPorGoogleClassroomIdQueryValidator()
        {
            RuleFor(c => c.GoogleClassroomId)
                .NotEmpty()
                .WithMessage("A Google Classroom id deve ser informada.");
        }
    }
}