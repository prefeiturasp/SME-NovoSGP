using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEPorEstudanteQuery : IRequest<EncaminhamentoAEEResumoDto>
    {
        public ObterEncaminhamentoAEEPorEstudanteQuery(string codigoEstudante)
        {
            CodigoEstudante = codigoEstudante;
        }

        public string CodigoEstudante { get; }
    }

    public class ObterEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ObterEncaminhamentoAEEPorEstudanteQuery>
    {
        public ObterEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");
        }
    }
}
