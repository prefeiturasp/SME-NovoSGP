using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }
        public string MotivoDevolucao { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
        public ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand(long encaminhamentoAEEId, string usuarioRF, string usuarioNome, string motivoDevolucao)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            UsuarioRF = usuarioRF;
            UsuarioNome = usuarioNome;
            MotivoDevolucao = motivoDevolucao;
        }

        public class ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandValidator : AbstractValidator<ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand>
        {
            public ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandValidator()
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

                RuleFor(c => c.MotivoDevolucao)
                  .NotEmpty()
                  .WithMessage("O motivo da devolução precisa ser informado.");
            }
        }
    }
}
