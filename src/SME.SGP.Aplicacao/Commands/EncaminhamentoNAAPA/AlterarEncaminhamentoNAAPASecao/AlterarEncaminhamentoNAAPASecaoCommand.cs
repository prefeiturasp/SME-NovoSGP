using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoNAAPASecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarEncaminhamentoNAAPASecaoCommand(EncaminhamentoNAAPASecao secao)
        {
            Secao = secao;
        }

        public EncaminhamentoNAAPASecao Secao { get; set; }
    }

    public class AlterarEncaminhamentoNAAPASecaoCommandValidator : AbstractValidator<AlterarEncaminhamentoNAAPASecaoCommand>
    {
        public AlterarEncaminhamentoNAAPASecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
                   .NotEmpty()
                   .WithMessage("A seção deve ser informada para a alteração do encaminhamento NAAPA!");
        }
    }
}
