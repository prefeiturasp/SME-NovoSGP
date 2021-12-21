using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosReprocessamentoConsolidacaoConselhoQuery : IRequest<IEnumerable<objConsolidacaoConselhoAluno>>
    {
        public int DreId { get; set; }

        public ObterAlunosReprocessamentoConsolidacaoConselhoQuery(int dreId)
        {
            DreId = dreId;
        }

        public class ObterAlunosReprocessamentoConsolidacaoConselhoQueryValidator : AbstractValidator<ObterAlunosReprocessamentoConsolidacaoConselhoQuery>
        {
            public ObterAlunosReprocessamentoConsolidacaoConselhoQueryValidator()
            {
                RuleFor(c => c.DreId)
                    .NotEmpty()
                    .WithMessage("O id da DRE deve ser informado.");
            }
        }
    }
}
