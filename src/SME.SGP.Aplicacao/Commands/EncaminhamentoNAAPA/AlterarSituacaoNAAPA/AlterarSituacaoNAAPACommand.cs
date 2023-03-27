using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoNAAPACommand : IRequest<bool>
    {
        public EncaminhamentoNAAPA Encaminhamento { get; set; }
        public SituacaoNAAPA Situacao { get; set; }

        public AlterarSituacaoNAAPACommand(EncaminhamentoNAAPA encaminhamentoId, SituacaoNAAPA situacao)
        {
            Encaminhamento = encaminhamentoId;
            Situacao = situacao;
        }
    }

    public class AlterarSituacaoNAAPACommandValidator : AbstractValidator<AlterarSituacaoNAAPACommand>
    {
        public AlterarSituacaoNAAPACommandValidator()
        {
            RuleFor(a => a.Encaminhamento)
                .NotEmpty()
                .WithMessage("É necessário informar o encaminhamento NAAPA para poder realizar a alteração da situação.");

            RuleFor(a => a.Situacao)
                .NotEmpty()
                .WithMessage("É necessário informar a nova situação do encaminhamento NAAPA para poder realizar a alteração.");
        }
    }
}
