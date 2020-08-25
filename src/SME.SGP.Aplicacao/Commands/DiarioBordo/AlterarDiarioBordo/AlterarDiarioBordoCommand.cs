using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarDiarioBordoCommand : IRequest<AuditoriaDto>
    {
        public AlterarDiarioBordoCommand(long id, long aulaId, string planejamento, string reflexoesReplanejamento)
        {
            Id = id;
            AulaId = aulaId;
            Planejamento = planejamento;
            ReflexoesReplanejamento = reflexoesReplanejamento;
        }

        public long Id { get; set; }
        public long AulaId { get; set; }
        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
    }

    public class AlterarDiarioBordoCommandValidator : AbstractValidator<AlterarDiarioBordoCommand>
    {
        public AlterarDiarioBordoCommandValidator()
        {
            RuleFor(a => a.Id)
                   .NotEmpty()
                   .GreaterThan(0)
                   .WithMessage("O Id do Diário de Bordo deve ser informado!");

            RuleFor(a => a.AulaId)
                   .NotEmpty()
                   .WithMessage("A aula deve ser informada!");

            RuleFor(a => a.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro do diário de bordo!")
                   .MinimumLength(200).WithMessage("O planejamento deve conter no mínimo 200 caracteres!");
        }
    }
}
