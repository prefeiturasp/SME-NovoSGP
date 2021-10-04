using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaConselhoEmAprovacaoQuery : IRequest<double>
    {
        public ObterNotaConselhoEmAprovacaoQuery(long conselhoClasseNotaId)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
        }
        public long ConselhoClasseNotaId { get; set; }
    }

    public class ObterNotaConselhoEmAprovacaoQueryValidator : AbstractValidator<ObterNotaConselhoEmAprovacaoQuery>
    {
        public ObterNotaConselhoEmAprovacaoQueryValidator()
        {
            RuleFor(a => a.ConselhoClasseNotaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id do Conselho de Ck");
        }
    }
}
