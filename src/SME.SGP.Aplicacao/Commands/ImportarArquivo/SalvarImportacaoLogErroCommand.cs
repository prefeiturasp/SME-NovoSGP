using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo
{
    public class SalvarImportacaoLogErroCommand : IRequest<ImportacaoLogErro>
    {
        public SalvarImportacaoLogErroCommand(SalvarImportacaoLogErroDto importacaoLogErro)
        {
            ImportacaoLogErro = importacaoLogErro;
        }

        public SalvarImportacaoLogErroDto ImportacaoLogErro { get; }
    }

    public class SalvarImportacaoLogErroCommandValidator : AbstractValidator<SalvarImportacaoLogErroCommand>
    {
        public SalvarImportacaoLogErroCommandValidator()
        {
            RuleFor(x => x.ImportacaoLogErro.ImportacaoLogId)
                .NotEmpty()
                .WithMessage("É necessário informar o ID de importação.");

            RuleFor(x => x.ImportacaoLogErro.LinhaArquivo)
                .NotEmpty()
                .WithMessage("É necessário informar a linha do arquivo.")
                .Must(l => int.TryParse(l.ToString(), out _))
                .WithMessage("A linha do arquivo deve ser um número inteiro válido.")
                .GreaterThan(0)
                .WithMessage("A linha do arquivo deve ser maior que zero.");

            RuleFor(x => x.ImportacaoLogErro.MotivoFalha)
                .NotEmpty()
                .WithMessage("É necessário informar o motivo da falha.");
        }
    }
}
