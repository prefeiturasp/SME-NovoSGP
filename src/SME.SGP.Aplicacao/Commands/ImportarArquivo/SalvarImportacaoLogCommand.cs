using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo
{
    public class SalvarImportacaoLogCommand : IRequest<ImportacaoLog>
    {
        public SalvarImportacaoLogCommand(SalvarImportacaoLogDto importacaoLog)
        {
            ImportacaoLog = importacaoLog;
        }

        public SalvarImportacaoLogDto ImportacaoLog { get; }
    }

    public class SalvarImportacaoLogCommandValidator : AbstractValidator<SalvarImportacaoLogCommand>
    {
        public SalvarImportacaoLogCommandValidator()
        {
            RuleFor(x => x.ImportacaoLog.NomeArquivo)
            .NotEmpty()
            .WithMessage("É necessário informar o nome do arquivo para inserir importação de arquivo.");

            RuleFor(x => x.ImportacaoLog.TipoArquivoImportacao)
                .NotEmpty()
                .WithMessage("É necessário informar o tipo do arquivo para importação.");

            RuleFor(x => x.ImportacaoLog.StatusImportacao)
                .NotEmpty()
                .WithMessage("É necessário informar o status para importação.");
        }
    }
}
