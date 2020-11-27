using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(long ueId, long periodoEscolarId, Modalidade[] modalidade)
        {
            UeId = ueId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long UeId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int[] Modalidades { get; set; }
    }

    public class ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryValidator : AbstractValidator<ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery>
    {
        public ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryValidator()
        {
            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta da situação de fechamento de conselho de classe das turmas.");

            RuleFor(c => c.PeriodoEscolarId)
               .Must(a => a > 0)
               .WithMessage("O id do Periodo Escolar deve ser informado para consulta da situação de fechamento de conselho de classe das turmas.");

            RuleFor(c => c.Modalidades)
               .NotEmpty()
               .WithMessage("As modalidades devem ser informadas para consulta da situação de fechamento de conselho de classe das turmas.");
        }
    }
}
