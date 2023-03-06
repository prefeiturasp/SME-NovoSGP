using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoMinioCommand  : IRequest<bool>
    {
        public ExcluirArquivoMinioCommand(string arquivoNome,string bucketNome = "")
        {
            ArquivoNome = arquivoNome;
            BucketNome = bucketNome;
        }

        public string ArquivoNome { get; set; }
        public string BucketNome { get; set; }
    }
    public class ExcluirArquivoMinioCommandValidator : AbstractValidator<ExcluirArquivoMinioCommand>
    {
        public ExcluirArquivoMinioCommandValidator()
        {
            RuleFor(c => c.ArquivoNome)
                .NotEmpty()
                .WithMessage("Informe o nome do arquivo para exclusão.");

        }
    }
}