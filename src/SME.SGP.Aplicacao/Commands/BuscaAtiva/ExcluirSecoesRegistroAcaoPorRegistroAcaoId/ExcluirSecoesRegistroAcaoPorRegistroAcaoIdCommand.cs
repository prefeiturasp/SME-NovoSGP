using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand : IRequest<bool>
    {
        public ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand(long registroAcaoId)
        {
            RegistroAcaoId = registroAcaoId;
        }

        public long RegistroAcaoId { get; }
    }

    public class ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommandValidator : AbstractValidator<ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand>
    {
        public ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommandValidator()
        {
            RuleFor(c => c.RegistroAcaoId)
            .NotEmpty()
            .WithMessage("O id do registro de ação deve ser informado para exclusão de suas seções.");

        }
    }
}
