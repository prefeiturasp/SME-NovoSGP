using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaAtualizacaoSituacaoConselhoClasseCommand : IRequest<bool>
    {
        public PublicaFilaAtualizacaoSituacaoConselhoClasseCommand(long conselhoClasseId, Usuario usuario)
        {
            ConselhoClasseId = conselhoClasseId;
            Usuario = usuario;
        }

        public long ConselhoClasseId { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class ExecutaAtualizacaoSituacaoConselhoClasseCommandValidator : AbstractValidator<PublicaFilaAtualizacaoSituacaoConselhoClasseCommand>
    {
        public ExecutaAtualizacaoSituacaoConselhoClasseCommandValidator()
        {
            RuleFor(c => c.ConselhoClasseId)
               .Must(a => a > 0)
               .WithMessage("O id do conselho de classe deve ser informado para atualização de sua situação.");

            RuleFor(c => c.Usuario)
               .NotEmpty()
               .WithMessage("O usuário deve ser informado para atualização da situação do conselho de classe.");
        }
    }
}
