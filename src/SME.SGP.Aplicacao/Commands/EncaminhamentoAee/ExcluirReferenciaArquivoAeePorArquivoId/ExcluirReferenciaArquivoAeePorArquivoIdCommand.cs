using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoAeePorArquivoIdCommand : IRequest<bool>
    {
        public ExcluirReferenciaArquivoAeePorArquivoIdCommand(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; set; }
    }

    public class ExcluirReferenciaArquivoAeePorArquivoIdCommandValidator : AbstractValidator<ExcluirReferenciaArquivoAeePorArquivoIdCommand>
    {

        /// <summary>
        /// /
        /// </summary>

        public ExcluirReferenciaArquivoAeePorArquivoIdCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo deve ser informado para exclusão.");
        }
    }
}
