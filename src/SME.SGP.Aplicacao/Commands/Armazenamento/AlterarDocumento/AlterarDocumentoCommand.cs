using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoCommand : IRequest<AuditoriaDto>
    {
        public AlterarDocumentoCommand(long documentoId, Guid[] arquivosCodigos)
        {
            DocumentoId = documentoId;
            ArquivosCodigos = arquivosCodigos;
        }

        public long DocumentoId { get; set; }
        public Guid[] ArquivosCodigos { get; set; }
    }

    public class AlterarDocumentoCommandValidator : AbstractValidator<AlterarDocumentoCommand>
    {
        public AlterarDocumentoCommandValidator()
        {
            RuleFor(a => a.DocumentoId)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("O Id do Documento deve ser informado.");

            RuleFor(a => a.ArquivosCodigos)
                .NotNull()
                .WithMessage("Os códigos dos arquivos devem ser informados.");
        }
    }
}
