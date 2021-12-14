using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class CopiarArquivoCommand : IRequest<string>
    {
        public CopiarArquivoCommand(string nome, TipoArquivo tipoArquivoOriginal, TipoArquivo tipoArquivoDestino)
        {
            Nome = nome;
            TipoArquivoOriginal = tipoArquivoOriginal;
            TipoArquivoDestino = tipoArquivoDestino;
        }

        public string Nome { get; set; }
        public TipoArquivo TipoArquivoOriginal { get; set; }
        public TipoArquivo TipoArquivoDestino { get; set; }
    }
        
    public class CopiarArquivoCommandValidator : AbstractValidator<CopiarArquivoCommand>
    {
        public CopiarArquivoCommandValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O Nome do Arquivo deve ser informado para registrar no repositório.");

            RuleFor(c => c.TipoArquivoOriginal)
                .NotEmpty()
                .WithMessage("O Tipo do Arquivo original deve ser informado para registrar no repositório.");

            RuleFor(c => c.TipoArquivoDestino)
               .NotEmpty()
               .WithMessage("O Tipo do Arquivo de destino deve ser informado para registrar no repositório.");
        }
    }
}
