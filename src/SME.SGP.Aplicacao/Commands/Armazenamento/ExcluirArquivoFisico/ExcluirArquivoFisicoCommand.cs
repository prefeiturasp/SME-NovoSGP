using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoFisicoCommand : IRequest<bool>
    {
        public ExcluirArquivoFisicoCommand(Guid codigo, TipoArquivo tipo, string nome)
        {
            Codigo = codigo;
            Tipo = tipo;
            Nome = nome;
        }

        public Guid Codigo { get; set; }
        public TipoArquivo Tipo { get; set; }
        public string Nome { get; set; }
    }

    public class ExcluirArquivoFisicoCommandValidator : AbstractValidator<ExcluirArquivoFisicoCommand>
    {
        public ExcluirArquivoFisicoCommandValidator()
        {
            RuleFor(c => c.Codigo)
            .Must(a => a != Guid.Empty)
            .WithMessage("O código do arquivo deve ser informado para exclusão física.");

            RuleFor(c => c.Tipo)
            .NotEmpty()
            .WithMessage("O tipo do arquivo deve ser informado para exclusão física.");

            RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para exclusão física.");
        }
    }
}
