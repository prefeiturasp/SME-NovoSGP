using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroAcaoPorSecaoIdCommand : IRequest<bool>
    {
        public ExcluirRegistroAcaoPorSecaoIdCommand(long registroAcaoSecaoId)
        {
            RegistroAcaoSecaoId = registroAcaoSecaoId;
        }

        public long RegistroAcaoSecaoId { get; }
    }

    public class ExcluirRegistroAcaoPorSecaoIdCommandValidator : AbstractValidator<ExcluirRegistroAcaoPorSecaoIdCommand>
    {
        public ExcluirRegistroAcaoPorSecaoIdCommandValidator()
        {
            RuleFor(c => c.RegistroAcaoSecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do registro de ação deve ser informado para exclusão de suas questões.");
        }
    }
}
