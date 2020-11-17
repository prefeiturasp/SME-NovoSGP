using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivoPorIdQuery : IRequest<Arquivo>
    {
        public ObterArquivoPorIdQuery(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; set; }
    }

    public class ObterArquivoPorIdQueryValidator : AbstractValidator<ObterArquivoPorIdQuery>
    {
        public ObterArquivoPorIdQueryValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo deve ser informado para obter seus dados.");

        }
    }
}
