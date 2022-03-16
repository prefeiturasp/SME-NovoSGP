using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand : IRequest<bool>
    {
        public ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand(int anoLetivo, long turmaId, DateTime dataAula, TipoPeriodoDashboardFrequencia tipoPeriodo, DateTime? dataInicioSemana, DateTime? dataFinalSemena, int? mes)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            DataAula = dataAula;
            TipoPeriodo = tipoPeriodo;
            DataInicioSemana = dataInicioSemana;
            DataFinalSemena = dataFinalSemena;
            Mes = mes;
        }

        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
        public DateTime? DataInicioSemana { get; set; }
        public DateTime? DataFinalSemena { get; set; }
        public int? Mes { get; set; }
    }
    public class ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommandValidator : AbstractValidator<ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand>
    {
        public ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para realizar a exclusão");

            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para realizar a exclusão");

            RuleFor(a => a.TipoPeriodo)
                .NotEmpty()
                .WithMessage("O tipo do período deve ser informado para realizar a exclusão");
        }
    }
}
