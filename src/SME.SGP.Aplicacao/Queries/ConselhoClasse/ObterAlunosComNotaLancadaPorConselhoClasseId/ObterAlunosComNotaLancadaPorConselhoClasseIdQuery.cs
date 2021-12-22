using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComNotaLancadaPorConselhoClasseIdQuery : IRequest<IEnumerable<string>>
    {
        public long ConselhoClasseId { get; set; }
        public ObterAlunosComNotaLancadaPorConselhoClasseIdQuery(long conselhoClasseId)
        {
            ConselhoClasseId = conselhoClasseId;
        }

        public class ObterAlunosComNotaLancadaPorConselhoClasseIdQueryValidator : AbstractValidator<ObterAlunosComNotaLancadaPorConselhoClasseIdQuery>
        {
            public ObterAlunosComNotaLancadaPorConselhoClasseIdQueryValidator()
            {
                RuleFor(c => c.ConselhoClasseId)
                    .NotEmpty()
                    .WithMessage("O código do Conselho de classe deve ser informado.");
            }
        }
    }
}
