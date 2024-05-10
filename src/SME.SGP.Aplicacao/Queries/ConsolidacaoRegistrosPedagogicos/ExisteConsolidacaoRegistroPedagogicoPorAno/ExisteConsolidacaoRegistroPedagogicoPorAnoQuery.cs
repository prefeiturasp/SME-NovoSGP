using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoRegistroPedagogicoPorAnoQuery : IRequest<bool>
    {
        public ExisteConsolidacaoRegistroPedagogicoPorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class ExisteConsolidacaoRegistroPedagogicoPorAnoQueryValidator : AbstractValidator<ExisteConsolidacaoRegistroPedagogicoPorAnoQuery>
    {
        public ExisteConsolidacaoRegistroPedagogicoPorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para verificar a existência de consolidação de registros pedagogicos");
        }
    }
}
