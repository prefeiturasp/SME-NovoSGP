using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirDiarioBordoCommand: IRequest<AuditoriaDto>
    {
        public InserirDiarioBordoCommand(long aulaId, string planejamento, string reflexoesReplanejamento)
        {
            AulaId = aulaId;
            Planejamento = planejamento;
            ReflexoesReplanejamento = reflexoesReplanejamento;
        }

        public long AulaId { get; set; }
        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
    }

    public class InserirDiarioBordoCommandValidator: AbstractValidator<InserirDiarioBordoCommand>
    {
        public InserirDiarioBordoCommandValidator()
        {
            RuleFor(a => a.AulaId)
                   .NotEmpty()
                   .WithMessage("A aula deve ser informada!");

            RuleFor(a => a.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro do diário de bordo!")
                   .MinimumLength(200).WithMessage("O planejamento deve conter no mínimo 200 caracteres!");
        }
    }
}
