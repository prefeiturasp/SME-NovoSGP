using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoPAPPorArquivoIdCommand : IRequest<bool>
    {
        public ExcluirReferenciaArquivoPAPPorArquivoIdCommand(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; set; }
    }

    public class ExcluirReferenciaArquivoPAPPorArquivoIdCommandValidator : AbstractValidator<ExcluirReferenciaArquivoPAPPorArquivoIdCommand>
    {
        public ExcluirReferenciaArquivoPAPPorArquivoIdCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
            .NotEmpty()
            .WithMessage("O id do arquivo deve ser informado para exclusão.");
        }
    }
}
