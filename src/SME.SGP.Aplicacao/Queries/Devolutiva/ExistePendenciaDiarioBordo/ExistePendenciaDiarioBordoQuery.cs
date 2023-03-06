using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaDiarioBordoQuery : IRequest<bool>
    {
        public ExistePendenciaDiarioBordoQuery(long turmaId, string componenteCodigo)
        {
            TurmaId = turmaId;
            ComponenteCodigo = componenteCodigo;
        }

        public long TurmaId { get; set;}
        public string ComponenteCodigo { get; set; }
    }
    public class ExistePendenciaDiarioBordoQueryValidator : AbstractValidator<ExistePendenciaDiarioBordoQuery>
    {
        public ExistePendenciaDiarioBordoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Turma Código deve ser informado");

            RuleFor(a => a.ComponenteCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Componente Código deve ser informado");
        }
    }
}
