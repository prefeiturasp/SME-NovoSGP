using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class DeletarArquivoDeRegistroExcluidoCommand : IRequest<bool>
    {
        public DeletarArquivoDeRegistroExcluidoCommand(string arquivoAtual, string caminho)
        {
            ArquivoAtual = arquivoAtual;
            Caminho = caminho;
        }

        public string ArquivoAtual { get; set; }
        public string Caminho { get; set; }
    }
    public class DeletarArquivoDeRegistroExcluidoCommandValidator : AbstractValidator<DeletarArquivoDeRegistroExcluidoCommand>
    {
        public DeletarArquivoDeRegistroExcluidoCommandValidator()
        {
            RuleFor(a => a.ArquivoAtual)
                .NotEmpty()
                .WithMessage("O registro Arquivo Atual deve ser informado");
            RuleFor(a => a.Caminho)
                .NotEmpty()
                .WithMessage("O Caminho deve ser informado");
        }
    }
}
