using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarSituacaoNovoEncaminhamentoNaapa
{
    public class AlterarSituacaoNovoEncaminhamentoNaapaCommand : IRequest<bool>
    {
        public EncaminhamentoEscolar Encaminhamento { get; set; }
        public SituacaoNAAPA Situacao { get; set; }

        public AlterarSituacaoNovoEncaminhamentoNaapaCommand(EncaminhamentoEscolar encaminhamentoId, SituacaoNAAPA situacao)
        {
            Encaminhamento = encaminhamentoId;
            Situacao = situacao;
        }
    }

    public class AlterarSituacaoNovoEncaminhamentoNaapaCommandValidator : AbstractValidator<AlterarSituacaoNovoEncaminhamentoNaapaCommand>
    {
        public AlterarSituacaoNovoEncaminhamentoNaapaCommandValidator()
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