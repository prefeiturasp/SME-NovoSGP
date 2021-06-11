using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoIdPorCodigoQuery : IRequest<long>
    {
        public ObterArquivoIdPorCodigoQuery(Guid arquivoCodigo)
        {
            ArquivoCodigo = arquivoCodigo;
        }

        public Guid ArquivoCodigo { get; }
    }

    public class ObterArquivoIdPorCodigoQueryValidator : AbstractValidator<ObterArquivoIdPorCodigoQuery>
    {
        public ObterArquivoIdPorCodigoQueryValidator()
        {
            RuleFor(c => c.ArquivoCodigo)
            .NotEmpty()
            .WithMessage("O Código do Arquivo deve ser informado para sua pesquisa.");

        }
    }
}
