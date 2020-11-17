using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoPorCodigoQuery : IRequest<Arquivo>
    {
        public ObterArquivoPorCodigoQuery(Guid codigo)
        {
            Codigo = codigo;
        }

        public Guid Codigo { get; set; }
    }

    public class ObterArquivoPorCodigoQueryValidator : AbstractValidator<ObterArquivoPorCodigoQuery>
    {
        public ObterArquivoPorCodigoQueryValidator()
        {
            RuleFor(c => c.Codigo)
            .NotEmpty()
            .WithMessage("O código do arquivo deve ser informado para obter seus dados.");

        }
    }
}
