using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand : IRequest<bool>
    {
        public ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(long documentoId, long arquivoId)
        {
            DocumentoId = documentoId;
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; set; }
        public long DocumentoId { get; set; }
    }

    public class ExcluirReferenciaArquivoDocumentoPorCodigoArquivoCommandValidator : AbstractValidator<ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand>
    {
        public ExcluirReferenciaArquivoDocumentoPorCodigoArquivoCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do documento  deve ser informado para exclusão.");

            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo  deve ser informado para exclusão.");

        }
    }
}
