﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommand : IRequest
    {
        public SalvarPendenciaDiarioBordoCommand()
        {}

        public ProfessorEComponenteInfantilDto ProfessorComponente { get; set; }
        public AulaComComponenteDto Aula { get; set; }
        public string CodigoTurma { get; set; }
        public string TurmaComModalidade { get; set; }
        public string NomeEscola { get; set; }
    }

    public class SalvarPendenciaDiarioBordoCommandValidator : AbstractValidator<SalvarPendenciaDiarioBordoCommand>
    {
        public SalvarPendenciaDiarioBordoCommandValidator()
        {
            RuleFor(c => c.Aula)
            .NotEmpty()
            .WithMessage("As aulas devem ser informados para geração de pendência diário de bordo.");

            RuleFor(c => c.ProfessorComponente)
            .NotEmpty()
            .WithMessage("A relação de professores devem ser informados para geração de pendência diário de bordo.");

            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para geração de pendência diário de bordo.");

            RuleFor(c => c.TurmaComModalidade)
            .NotEmpty()
            .WithMessage("O nome da turma com modalidade deve ser informado para geração de pendência diário de bordo.");

            RuleFor(c => c.NomeEscola)
            .NotEmpty()
            .WithMessage("O nome da escola deve ser informado para geração de pendência diário de bordo.");
        }
    }
}
