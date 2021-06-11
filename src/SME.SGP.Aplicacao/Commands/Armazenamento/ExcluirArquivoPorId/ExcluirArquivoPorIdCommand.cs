using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoPorIdCommand : IRequest<bool>
    {
        public ExcluirArquivoPorIdCommand(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; }
    }

    public class ExcluirArquivoPorIdCommandValidator : AbstractValidator<ExcluirArquivoPorIdCommand>
    {
        public ExcluirArquivoPorIdCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo deve ser informado para exclusão.");
        }
    }
}
