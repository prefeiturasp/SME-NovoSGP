using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoAvaliacaoPorCodigoQuery : IRequest<long>
    {
        public ObterTipoAvaliacaoPorCodigoQuery(int codigo)
        {
            Codigo = codigo;
        }

        public int Codigo { get; }
    }

    public class ObterTipoAvaliacaoPorCodigoQueryValidator : AbstractValidator<ObterTipoAvaliacaoPorCodigoQuery>
    {
        public ObterTipoAvaliacaoPorCodigoQueryValidator()
        {
            RuleFor(a => a.Codigo)
                .NotEmpty()
                .WithMessage("O código do tipo de avaliação deve ser informado");
        }
    }
}
