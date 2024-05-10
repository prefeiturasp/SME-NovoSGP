using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoConselhoClasseQuery : IRequest<SituacaoConselhoClasse>
    {
        public ObterSituacaoConselhoClasseQuery(long turmaId, long periodoEscolarId)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long TurmaId { get; set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ObterSituacaoConselhoClasseQueryValidator : AbstractValidator<ObterSituacaoConselhoClasseQuery>
    {
        public ObterSituacaoConselhoClasseQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta da situação do conselho de classe.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O id do período escolar deve ser informado para consulta da situação do conselho de classe.");
        }
    }
}
