using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesRegistrosPedagogicosCommand : IRequest
    {
        public RemoverConsolidacoesRegistrosPedagogicosCommand(ConsolidacaoRegistrosPedagogicos consolidacao)
        {
            ConsolidacaoRegistrosPedagogicos = consolidacao;
        }
        public ConsolidacaoRegistrosPedagogicos ConsolidacaoRegistrosPedagogicos { get; set; }

    }

    public class RemoverConsolidacoesRegistrosPedagogicosCommandValidator : AbstractValidator<RemoverConsolidacoesRegistrosPedagogicosCommand>
    {
        public RemoverConsolidacoesRegistrosPedagogicosCommandValidator()
        {
            RuleFor(a => a.ConsolidacaoRegistrosPedagogicos)
                .NotEmpty()
                .WithMessage("Os dados da consolidação devem ser informados para o registro.");
        }
    }
}
