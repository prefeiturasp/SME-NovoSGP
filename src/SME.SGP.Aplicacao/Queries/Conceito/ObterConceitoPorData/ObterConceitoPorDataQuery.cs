using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitoPorDataQuery : IRequest<IEnumerable<Conceito>>
    {
        public ObterConceitoPorDataQuery(DateTime dataAvaliacao)
        {
            DataAvaliacao = dataAvaliacao;
        }

        public DateTime DataAvaliacao { get; set; }
    }

    public class ObterConceitoPorDataQueryValidator : AbstractValidator<ObterConceitoPorDataQuery>
    {
        public ObterConceitoPorDataQueryValidator()
        {
            RuleFor(x => x.DataAvaliacao)
                .NotEmpty().WithMessage("Informe a Data da Avaliação para Obter Conceito Por Data");
        }
    }
}