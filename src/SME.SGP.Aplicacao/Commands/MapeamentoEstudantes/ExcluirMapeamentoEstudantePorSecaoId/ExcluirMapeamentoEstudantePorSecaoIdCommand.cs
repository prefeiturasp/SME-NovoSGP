using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirMapeamentoEstudantePorSecaoIdCommand : IRequest<bool>
    {
        public ExcluirMapeamentoEstudantePorSecaoIdCommand(long mapeamentoEstudanteSecaoId)
        {
            MapeamentoEstudanteSecaoId = mapeamentoEstudanteSecaoId;
        }

        public long MapeamentoEstudanteSecaoId { get; }
    }

    public class ExcluirMapeamentoEstudantePorSecaoIdCommandValidator : AbstractValidator<ExcluirMapeamentoEstudantePorSecaoIdCommand>
    {
        public ExcluirMapeamentoEstudantePorSecaoIdCommandValidator()
        {
            RuleFor(c => c.MapeamentoEstudanteSecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do mapeamento estudante deve ser informado para exclusão de suas questões.");
        }
    }
}
