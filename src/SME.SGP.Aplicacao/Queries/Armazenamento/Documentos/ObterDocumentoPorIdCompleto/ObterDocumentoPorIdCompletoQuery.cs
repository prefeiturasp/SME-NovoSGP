using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentoPorIdCompletoQuery : IRequest<ObterDocumentoDto>
    {
        public ObterDocumentoPorIdCompletoQuery(long documentoId)
        {
            DocumentoId = documentoId;
        }

        public long DocumentoId { get; set; }
    }

    public class ObterDocumentoPorIdCompletoQueryValidator : AbstractValidator<ObterDocumentoPorIdCompletoQuery>
    {
        public ObterDocumentoPorIdCompletoQueryValidator()
        {
            RuleFor(c => c.DocumentoId)
            .NotEmpty()
            .WithMessage("O id do documento deve ser informado para obter seus dados.");

        }
    }
}
