using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseConsolidadoTurmaAlunoCommand : IRequest<long>
    {
        public SalvarConselhoClasseConsolidadoTurmaAlunoCommand(ConselhoClasseConsolidadoTurmaAluno conselhoClasseConsolidadoTurmaAluno)
        {
            ConselhoClasseConsolidadoTurmaAluno = conselhoClasseConsolidadoTurmaAluno ?? throw new ArgumentNullException(nameof(conselhoClasseConsolidadoTurmaAluno));
        }

        public ConselhoClasseConsolidadoTurmaAluno ConselhoClasseConsolidadoTurmaAluno { get; }
    }

    public class SalvarConselhoClasseConsolidadoTurmaAlunoCommandValidator : AbstractValidator<SalvarConselhoClasseConsolidadoTurmaAlunoCommand>
    {
        public SalvarConselhoClasseConsolidadoTurmaAlunoCommandValidator()
        {
            RuleFor(c => c.ConselhoClasseConsolidadoTurmaAluno)
               .NotNull()
               .WithMessage("O consolidado do conselho de classe turma aluno deve ser informado para efetuar a gravação.");
        }
    }
}
