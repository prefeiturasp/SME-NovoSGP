using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class DeletarArquivoPastaTempCommand : IRequest<bool>
    {
        public DeletarArquivoPastaTempCommand(string arquivoAtual, string arquivoNovo, string caminho)
        {
            ArquivoAtual = arquivoAtual;
            ArquivoNovo = arquivoNovo;
            Caminho = caminho;
        }

        public string ArquivoAtual { get; set; }
        public string ArquivoNovo { get; set; }
        public string Caminho { get; set; }
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
