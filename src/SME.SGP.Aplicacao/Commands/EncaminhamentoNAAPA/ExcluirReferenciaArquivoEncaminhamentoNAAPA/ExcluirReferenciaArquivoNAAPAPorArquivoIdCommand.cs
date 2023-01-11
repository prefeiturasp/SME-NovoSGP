﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoNAAPAPorArquivoIdCommand : IRequest<bool>
    {
        public ExcluirReferenciaArquivoNAAPAPorArquivoIdCommand(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; set; }
    }

    public class ExcluirReferenciaArquivoNAAPAPorArquivoIdCommandValidator : AbstractValidator<ExcluirReferenciaArquivoNAAPAPorArquivoIdCommand>
    {
        public ExcluirReferenciaArquivoNAAPAPorArquivoIdCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo deve ser informado para exclusão.");
        }
    }
}
