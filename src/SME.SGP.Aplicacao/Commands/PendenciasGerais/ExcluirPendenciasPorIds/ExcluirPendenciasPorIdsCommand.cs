using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasPorIdsCommand : IRequest<bool>
    {
        public long[] PendenciasIds { get; set; }
    }

    public class ExcluirPendenciasPorIdsCommandValidator : AbstractValidator<ExcluirPendenciasPorIdsCommand>
    {
        public ExcluirPendenciasPorIdsCommandValidator()
        {
            RuleFor(a => a.PendenciasIds)
                .NotEmpty()
                .WithMessage("Será necessário informar os id's das pendências a serem excluídas.");
        }
    }
}
