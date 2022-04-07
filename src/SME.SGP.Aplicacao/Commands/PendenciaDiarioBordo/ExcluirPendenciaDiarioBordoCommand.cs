using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaDiarioBordo
{
    public class ExcluirPendenciaDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirPendenciaDiarioBordoCommand(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ExcluirPendenciaDiarioBordoCommandValidator : AbstractValidator<ExcluirPendenciaDiarioBordoCommand>
    {
        public ExcluirPendenciaDiarioBordoCommandValidator()
        {
            RuleFor(x => x.PendenciaId)
                    .NotEmpty()
                    .WithMessage("A Pendencia deve ser informada.");
        }
    }
}
