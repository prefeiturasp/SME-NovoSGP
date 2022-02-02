using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitoPorIdQuery : IRequest<Conceito>
    {
        public long Id { get; set; }

        public ObterConceitoPorIdQuery(long id)
        {
            Id = id;
        }
    }

    public class ObterConceitoPorIdQueryValidator : AbstractValidator<ObterConceitoPorIdQuery>
    {
        public ObterConceitoPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("É necessário informar o id do conceito para obter os dados do mesmo.");
        }
    }
}
