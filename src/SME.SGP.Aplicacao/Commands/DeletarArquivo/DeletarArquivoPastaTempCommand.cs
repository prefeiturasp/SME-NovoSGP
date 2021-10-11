using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class DeletarArquivoPastaTempCommand : IRequest<bool>
    {
        public DeletarArquivoPastaTempCommand(string arquivoAtual, string arquivoNovo)
        {
            ArquivoAtual = arquivoAtual;
            ArquivoNovo = arquivoNovo;
        }

        public string ArquivoAtual { get; set; }
        public string ArquivoNovo { get; set; }
    }
    public class DeletarArquivoPastaTempCommandValidator : AbstractValidator<DeletarArquivoPastaTempCommand>
    {
        public DeletarArquivoPastaTempCommandValidator()
        {
            RuleFor(a => a.ArquivoAtual)
                .NotEmpty()
                .WithMessage("O registro Arquivo Atual deve ser informado");
            RuleFor(a => a.ArquivoNovo)
            .NotEmpty()
            .WithMessage("O registro Arquivo Novo  deve ser informado");
        }
    }
}
