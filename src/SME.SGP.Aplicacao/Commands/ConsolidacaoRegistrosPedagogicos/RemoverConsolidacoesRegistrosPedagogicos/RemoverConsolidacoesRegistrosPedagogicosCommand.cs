using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesRegistrosPedagogicosCommand : IRequest
    {
        public RemoverConsolidacoesRegistrosPedagogicosCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class RemoverConsolidacoesRegistrosPedagogicosCommandValidator : AbstractValidator<RemoverConsolidacoesRegistrosPedagogicosCommand>
    {
        public RemoverConsolidacoesRegistrosPedagogicosCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para exclusão das consolidações de registros pedagogicos");
        }
    }
}
