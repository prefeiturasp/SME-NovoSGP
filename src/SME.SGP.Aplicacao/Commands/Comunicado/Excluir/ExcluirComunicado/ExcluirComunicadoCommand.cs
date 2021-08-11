using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoCommand : IRequest<bool>
    {
        public long[] Ids { get; set; }

        public ExcluirComunicadoCommand(long[] ids)
        {
            Ids = ids;
        }
    }
    public class ExcluirComunicadoCommandValidator : AbstractValidator<ExcluirComunicadoCommand>
    {
        public ExcluirComunicadoCommandValidator()
        {

            RuleFor(c => c.Ids)
                .NotEmpty()
                .WithMessage("Pelo menos um comunicado deve ser informado.");           
        }
    }
}
