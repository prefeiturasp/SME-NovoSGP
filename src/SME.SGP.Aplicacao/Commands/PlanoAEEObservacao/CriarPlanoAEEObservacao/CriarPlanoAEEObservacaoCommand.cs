using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class CriarPlanoAEEObservacaoCommand : IRequest<AuditoriaDto>
    {
        public CriarPlanoAEEObservacaoCommand(long planoAEEId, string observacao, IEnumerable<long> usuarios)
        {
            PlanoAEEId = planoAEEId;
            Observacao = observacao;
        }

        public long PlanoAEEId { get; }
        public string Observacao { get; }
    }

    public class CriarPlanoAEEObservacaoCommandValidator : AbstractValidator<CriarPlanoAEEObservacaoCommand>
    {
        public CriarPlanoAEEObservacaoCommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para gerar observação");

            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada para registro");
        }
    }
}
