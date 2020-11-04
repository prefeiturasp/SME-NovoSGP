using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoRepositorioPorIdCommand : IRequest<bool>
    {
        public ExcluirArquivoRepositorioPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirArquivoRepositorioPorIdCommandValidator : AbstractValidator<ExcluirArquivoRepositorioPorIdCommand>
    {
        public ExcluirArquivoRepositorioPorIdCommandValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O identificador do arquivo deve ser informado para exclusão.");
        }
    }
}
