using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaQuestaoPorArquivoCommand : IRequest<bool>
    {
        public ExcluirItineranciaQuestaoPorArquivoCommand(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public  long ArquivoId { get; set; }
    }
    public class ExcluirItineranciaQuestaoPorArquivoCommandValidator : AbstractValidator<ExcluirItineranciaQuestaoPorArquivoCommand>
    {
        public ExcluirItineranciaQuestaoPorArquivoCommandValidator()
        {
            RuleFor(c => c.ArquivoId)
                .GreaterThan(0)
                .WithMessage("O id do Arquivo deve ser informado para exclusão.");
        }
    }
}