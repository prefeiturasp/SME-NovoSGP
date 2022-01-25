﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirDiarioBordoCommand: IRequest<AuditoriaDto>
    {
        public InserirDiarioBordoCommand(long aulaId, string planejamento, string reflexoesReplanejamento, long componenteCurricularId)
        {
            AulaId = aulaId;
            Planejamento = planejamento;
            ReflexoesReplanejamento = reflexoesReplanejamento;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
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
            RuleFor(a => a.ComponenteCurricularId)
                   .NotEmpty()
                   .WithMessage("O Id do Componente Curricular deve ser informado!");

            RuleFor(a => a.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro do diário de bordo!")
                   .MinimumLength(200).WithMessage("O planejamento deve conter no mínimo 200 caracteres!");
        }
    }
}
