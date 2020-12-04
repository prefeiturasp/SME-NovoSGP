using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComInicioFechamentoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasComInicioFechamentoQuery(long ueId, long periodoEscolarId, Modalidade[] modalidades)
        {
            UeId = ueId;
            PeriodoEscolarId = periodoEscolarId;
            Modalidades = modalidades.Cast<int>().ToArray();
        }

        public long UeId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int[] Modalidades { get; set; }
    }

    public class ObterTurmasComInicioFechamentoQueryValidator : AbstractValidator<ObterTurmasComInicioFechamentoQuery>
    {
        public ObterTurmasComInicioFechamentoQueryValidator()
        {
            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta de início de fechamento das turmas.");

            RuleFor(c => c.PeriodoEscolarId)
               .Must(a => a > 0)
               .WithMessage("O id do Periodo Escolar deve ser informado para consulta de início de fechamento das turmas.");

            RuleFor(c => c.Modalidades)
               .Must(a => a.Length > 0)
               .WithMessage("As modalidades devem ser informadas para consulta de início de fechamento das turmas.");
        }
    }
}
