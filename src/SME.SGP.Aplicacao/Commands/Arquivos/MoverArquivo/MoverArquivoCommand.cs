using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoCommand : IRequest<string>
    {
        public MoverArquivoCommand(string nome, TipoArquivo tipo)
        {
            Tipo = tipo;
            Nome = nome;
        }

        public string Nome { get; set; }
        public TipoArquivo Tipo { get; set; }
    }

    public class MoverArquivoCommandValidator : AbstractValidator<MoverArquivoCommand>
    {
        public MoverArquivoCommandValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O Nome do Arquivo deve ser informado para registrar no repositório.");

            RuleFor(c => c.Tipo)
                .NotEmpty()
                .WithMessage("O Tipo do Arquivo deve ser informado para registrar no repositório.");
        }
    }
}