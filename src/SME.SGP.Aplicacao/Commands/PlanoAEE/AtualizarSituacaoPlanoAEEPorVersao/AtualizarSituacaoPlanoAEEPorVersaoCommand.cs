using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoPlanoAEEPorVersaoCommand : IRequest<bool>
    {
        public AtualizarSituacaoPlanoAEEPorVersaoCommand(long versaoId, SituacaoPlanoAEE situacao)
        {
            VersaoId = versaoId;
            Situacao = situacao;
        }

        public long VersaoId { get; }
        public SituacaoPlanoAEE Situacao { get; }
    }

    public class AtualizarSituacaoPlanoAEEPorVersaoCommandValidator : AbstractValidator<AtualizarSituacaoPlanoAEEPorVersaoCommand>
    {
        public AtualizarSituacaoPlanoAEEPorVersaoCommandValidator()
        {
            RuleFor(a => a.VersaoId)
                .NotEmpty()
                .WithMessage("O id da versão do plano deve ser informado para atualização de sua situação");
        }
    }
}
