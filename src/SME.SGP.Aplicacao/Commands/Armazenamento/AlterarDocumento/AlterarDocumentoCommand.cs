using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoCommand : IRequest<AuditoriaDto>
    {
        public AlterarDocumentoCommand(long documentoId, Guid codigoArquivo)
        {
            DocumentoId = documentoId;
            CodigoArquivo = codigoArquivo;
        }

        public long DocumentoId { get; set; }
        public Guid CodigoArquivo { get; set; }
    }

    public class AlterarDocumentoCommandValidator : AbstractValidator<AlterarDocumentoCommand>
    {
        public AlterarDocumentoCommandValidator()
        {
            RuleFor(a => a.DocumentoId)
                   .NotEmpty()
                   .GreaterThan(0)
                   .WithMessage("O Id do Documento deve ser informado!");

            RuleFor(a => a.CodigoArquivo)
                   .NotEmpty()
                   .WithMessage("A código do arquivo ser informado!");
        }
    }
}
