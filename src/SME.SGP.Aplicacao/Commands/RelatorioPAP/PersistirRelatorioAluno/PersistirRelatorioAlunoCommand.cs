using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioAlunoCommand : IRequest<RelatorioPeriodicoPAPAluno>
    {
        public PersistirRelatorioAlunoCommand(RelatorioPAPDto relatorioPapDto, long relatorioTurmaId)
        {
            RelatorioPAPDto = relatorioPapDto;
            RelatorioTurmaId = relatorioTurmaId;
        }

        public RelatorioPAPDto RelatorioPAPDto { get; set; } 
        public long RelatorioTurmaId { get; set; } 
    }

    public class PersistirRelatorioAlunoCommandValidator : AbstractValidator<PersistirRelatorioAlunoCommand>
    {
        public PersistirRelatorioAlunoCommandValidator()
        {
            RuleFor(x => x.RelatorioTurmaId).GreaterThan(0).WithMessage("Informe o Id da Turma");
            RuleFor(x => x.RelatorioPAPDto).NotEmpty().WithMessage("Informe o relatório pap");
        }
    }
}