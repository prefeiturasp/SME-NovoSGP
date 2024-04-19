using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand : IRequest<bool>
    {
        public ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand(long mapeamentoEstudanteId)
        {
            MapeamentoEstudanteId = mapeamentoEstudanteId;
        }

        public long MapeamentoEstudanteId { get; }
    }

    public class ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommandValidator : AbstractValidator<ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand>
    {
        public ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommandValidator()
        {
            RuleFor(c => c.MapeamentoEstudanteId)
            .NotEmpty()
            .WithMessage("O id do mapeamento de estudante deve ser informado para exclusão de suas seções.");

        }
    }
}
