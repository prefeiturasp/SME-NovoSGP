using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
        public ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand(long encaminhamentoAEEId, string usuarioRF, string usuarioNome)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            UsuarioRF = usuarioRF;
            UsuarioNome = usuarioNome;
        }

        public class ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandValidator : AbstractValidator<ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand>
        {
            public ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandValidator()
            {
                RuleFor(c => c.EncaminhamentoAEEId)
                   .NotEmpty()
                   .WithMessage("O id do encaminhamento precisa ser informado.");

                RuleFor(c => c.UsuarioRF)
                   .NotEmpty()
                   .WithMessage("O rf do usuário precisa ser informado.");

                RuleFor(c => c.UsuarioNome)
                   .NotEmpty()
                   .WithMessage("O nome do usuário precisa ser informado.");
            }
        }
    }
}
