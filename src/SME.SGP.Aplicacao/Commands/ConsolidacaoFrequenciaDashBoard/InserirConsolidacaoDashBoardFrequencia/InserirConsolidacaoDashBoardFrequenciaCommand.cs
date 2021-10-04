using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoDashBoardFrequenciaCommand : IRequest<bool>
    {
        public InserirConsolidacaoDashBoardFrequenciaCommand(long turmaId, DateTime dataAula, TipoPeriodoDashboardFrequencia tipoPeriodo)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
            TipoPeriodo = tipoPeriodo;
        }

        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
    }

    public class InserirConsolidacaoDashBoardFrequenciaCommandValidator : AbstractValidator<InserirConsolidacaoDashBoardFrequenciaCommand>
    {
        public InserirConsolidacaoDashBoardFrequenciaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para realizar a consolidação");

            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para realizar a consolidação");

            RuleFor(a => a.TipoPeriodo)
                .NotEmpty()
                .WithMessage("o tipo do período deve ser informado para realizar a consolidação");
        }
    }
}
