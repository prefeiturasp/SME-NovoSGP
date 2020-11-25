using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExclusaoPendenciasAulaCommand : IRequest<bool>
    {
        public IncluirFilaExclusaoPendenciasAulaCommand(long aulaId, Usuario usuario)
        {
            AulaId = aulaId;
            Usuario = usuario;
        }

        public long AulaId { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class IncluirFilaExclusaoPendenciasAulaCommandValidator : AbstractValidator<IncluirFilaExclusaoPendenciasAulaCommand>
    {
        public IncluirFilaExclusaoPendenciasAulaCommandValidator()
        {
            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O id da aula deve ser informado para inclusão do processo de exclusão de pendências na fila.");

            RuleFor(c => c.Usuario)
            .NotEmpty()
            .WithMessage("O usuário deve ser informado para inclusão do processo de exclusão de pendências na fila.");
        }
    }
}
