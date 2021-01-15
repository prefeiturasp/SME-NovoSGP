using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaProfessorCommand : IRequest<bool>
    {
        public ExcluirPendenciaProfessorCommand(PendenciaProfessor pendenciaProfessor)
        {
            PendenciaProfessor = pendenciaProfessor;
        }

        public PendenciaProfessor PendenciaProfessor { get; set; }
    }

    public class ExcluirPendenciaProfessorCommandValidator : AbstractValidator<ExcluirPendenciaProfessorCommand>
    {
        public ExcluirPendenciaProfessorCommandValidator()
        {
            RuleFor(c => c.PendenciaProfessor)
               .NotEmpty()
               .WithMessage("A pendencia do professor deve ser informada para sua exclusão.");
        }
    }
}