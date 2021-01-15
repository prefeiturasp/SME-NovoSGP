using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(long ueId, int anoLetivo, long? periodoEscolarId, Modalidade[] modalidades, int semestre)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            PeriodoEscolarId = periodoEscolarId;
            Modalidades = modalidades.Cast<int>().ToArray();
            Semestre = semestre;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryValidator : AbstractValidator<ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery>
    {
        public ObterTurmasComFechamentoOuConselhoNaoFinalizadosQueryValidator()
        {
            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta da situação de fechamento de conselho de classe das turmas.");

            RuleFor(c => c.AnoLetivo)
              .Must(a => a > 0)
              .WithMessage("O Ano Letivo deve ser informado para consulta da situação de fechamento de conselho de classe das turmas.");

            RuleFor(c => c.Modalidades)
               .Must(a => a.Length > 0)
               .WithMessage("As modalidades devem ser informadas para consulta da situação de fechamento de conselho de classe das turmas.");
        }
    }
}
