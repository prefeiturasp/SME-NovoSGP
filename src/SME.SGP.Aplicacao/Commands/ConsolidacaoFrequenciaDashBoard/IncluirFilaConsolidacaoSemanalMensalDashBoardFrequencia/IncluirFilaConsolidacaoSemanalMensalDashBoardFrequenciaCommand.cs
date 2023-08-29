using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(long turmaId, string codigoTurma, bool ehModalidadeInfantil, int anoLetivo, DateTime dataAula)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
            EhModalidadeInfantil = ehModalidadeInfantil;
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
        }
        
        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public bool EhModalidadeInfantil { get; set; }
        public string CodigoTurma { get; set; }
        public int AnoLetivo { get; set; }
    }
    
    public class IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandValidator : AbstractValidator<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>
    {
        public IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para a consolidação semanal do dashboard frequência");

            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para a consolidação semanal do dashboard frequência");
            
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para a consolidação semanal do dashboard frequência");
            
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a consolidação semanal do dashboard frequência");
        }
    }
}
