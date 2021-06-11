using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEPorCodigoEstudanteQuery : IRequest<IEnumerable<PlanoAEEVersaoDto>>
    {
        public ObterVersoesPlanoAEEPorCodigoEstudanteQuery(string codigoEstudante)
        {
            CodigoEstudante = codigoEstudante;
        }

        public string CodigoEstudante { get; }
    }

    public class ObterVersoesPlanoAEEPorCodigoEstudanteQueryValidator : AbstractValidator<ObterVersoesPlanoAEEPorCodigoEstudanteQuery>
    {
        public ObterVersoesPlanoAEEPorCodigoEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante deve ser informado para consulta de suas versões.");
        }
    }
}
