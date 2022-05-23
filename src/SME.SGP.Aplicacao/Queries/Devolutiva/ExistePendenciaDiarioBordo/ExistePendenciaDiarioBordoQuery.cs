using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaDiarioBordoQuery : IRequest<bool>
    {
        public ExistePendenciaDiarioBordoQuery(string turmaCodigo, string componenteCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCodigo = componenteCodigo;
        }

        public string TurmaCodigo { get; set;}
        public string ComponenteCodigo { get; set; }
    }
    public class ExistePendenciaDiarioBordoQueryValidator : AbstractValidator<ExistePendenciaDiarioBordoQuery>
    {
        public ExistePendenciaDiarioBordoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
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
