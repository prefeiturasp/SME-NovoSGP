using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery : IRequest<RetornoConsolidacaoExistenteDto>
    {
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
        public int Mes { get; set; }

        public ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery(long turmaId, int anoLetivo, TipoPeriodoDashboardFrequencia tipoPeriodo, int mes)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            TipoPeriodo = tipoPeriodo;
            Mes = mes;
        }
    }

    public class ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQueryValidator : AbstractValidator<ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery>
    {
        public ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma para obter a consolidação do dashboard de frequência já existente dela.");

            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo da turma para obter a consolidação do dashboard de frequência já existente dela.");

            RuleFor(a => a.TipoPeriodo)
              .NotEmpty()
              .WithMessage("É necessário informar o tipo de período (diário, semanal ou mensal) do dashboard para obter a consolidação da frequência já existente dela.");

            RuleFor(a => a.Mes)
              .NotEmpty()
              .WithMessage("É necessário informar o mês de período de registro para obter a consolidação do dashboard de frequência já existente dela.");
        }
    }
}
