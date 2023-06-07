using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand : IRequest<long>
    {
        public SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand(ConselhoClasseConsolidadoTurmaAlunoNota conselhoClasseConsolidadoTurmaAlunoNota)
        {
            ConselhoClasseConsolidadoTurmaAlunoNota = conselhoClasseConsolidadoTurmaAlunoNota ?? throw new ArgumentNullException(nameof(conselhoClasseConsolidadoTurmaAlunoNota));
        }

        public ConselhoClasseConsolidadoTurmaAlunoNota ConselhoClasseConsolidadoTurmaAlunoNota { get; }
    }

    public class SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandValidator : AbstractValidator<SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand>
    {
        public SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandValidator()
        {
            RuleFor(c => c.ConselhoClasseConsolidadoTurmaAlunoNota)
               .NotNull()
               .WithMessage("O consolidado do conselho de classe turma aluno nota deve ser informado para efetuar a gravação.");
        }
    }
}
