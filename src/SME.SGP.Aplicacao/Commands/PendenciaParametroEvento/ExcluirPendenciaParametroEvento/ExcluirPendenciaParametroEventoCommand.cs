using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaParametroEventoCommand : IRequest<bool>
    {
        public ExcluirPendenciaParametroEventoCommand(PendenciaParametroEvento pendenciaParametroEvento)
        {
            PendenciaParametroEvento = pendenciaParametroEvento;
        }

        public PendenciaParametroEvento PendenciaParametroEvento { get; set; }
    }

    public class ExcluirPendenciaParametroEventoCommandValidator : AbstractValidator<ExcluirPendenciaParametroEventoCommand>
    {
        public ExcluirPendenciaParametroEventoCommandValidator()
        {
            RuleFor(c => c.PendenciaParametroEvento)
               .NotEmpty()
               .WithMessage("A pendencia parametro deve ser informada para exclusão.");
        }
    }
}
