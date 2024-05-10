using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoRepositorioPorCodigoCommand : IRequest<bool>
    {
        public ExcluirArquivoRepositorioPorCodigoCommand(Guid codigoArquivo)
        {
            CodigoArquivo = codigoArquivo;
        }

        public Guid CodigoArquivo { get; set; }
    }

    public class ExcluirArquivoRepositorioPorCodigoCommandValidator : AbstractValidator<ExcluirArquivoRepositorioPorCodigoCommand>
    {
        public ExcluirArquivoRepositorioPorCodigoCommandValidator()
        {
            RuleFor(c => c.CodigoArquivo)
            .NotEmpty()
            .WithMessage("O código do arquivo  deve ser informado para exclusão.");

        }
    }
}
