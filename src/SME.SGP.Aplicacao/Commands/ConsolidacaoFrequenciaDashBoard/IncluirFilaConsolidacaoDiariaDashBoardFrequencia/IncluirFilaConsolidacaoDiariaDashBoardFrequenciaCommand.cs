using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(long turmaId, DateTime dataAula)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
        }

        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
    }
    
    public class IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandValidator : AbstractValidator<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>
    {
        public IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para a consolidação diária do dashboard frequência");

            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para a consolidação diária do dashboard frequência");
        }
    }
}
