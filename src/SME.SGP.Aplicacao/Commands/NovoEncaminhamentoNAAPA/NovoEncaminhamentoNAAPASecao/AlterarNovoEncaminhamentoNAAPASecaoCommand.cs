using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.NovoEncaminhamentoNAAPASecao
{
    public class AlterarNovoEncaminhamentoNAAPASecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarNovoEncaminhamentoNAAPASecaoCommand(EncaminhamentoNAAPASecao secao)
        {
            Secao = secao;
        }

        public EncaminhamentoNAAPASecao Secao { get; }
    }

    public class AlterarNovoEncaminhamentoNAAPASecaoCommandValidator : AbstractValidator<AlterarNovoEncaminhamentoNAAPASecaoCommand>
    {
        public AlterarNovoEncaminhamentoNAAPASecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
               .NotEmpty()
               .WithMessage("A seção deve ser informada para a alteração do encaminhamento NAAPA!");
        }
    }
}