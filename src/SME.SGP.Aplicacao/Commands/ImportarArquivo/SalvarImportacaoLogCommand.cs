using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo
{
    public class SalvarImportacaoLogCommand : IRequest<ImportacaoLog>
    {
        public SalvarImportacaoLogCommand(ImportacaoLogDto importacaoLog)
        {
            ImportacaoLog = importacaoLog;
        }

        public ImportacaoLogDto ImportacaoLog { get; }
    }

    public class SalvarImportacaoLogCommandValidator : AbstractValidator<SalvarImportacaoLogCommand>
    {
        public SalvarImportacaoLogCommandValidator()
        {
            RuleFor(x => x.ImportacaoLog)
                .NotNull().WithMessage("É necessário enviar um arquivo para importação.")
                .Must(a => !a.Arquivo.ContentType.NaoEhArquivoXlsx())
                .WithMessage("Somente arquivos no formato XLSX são suportados.");

            RuleFor(x => x.ImportacaoLog.NomeArquivo)
            .NotEmpty()
            .WithMessage("É necessário informar o nome do arquivo para inserir importação de arquivo.");

            //RuleFor(x => x.ImportacaoLog.AnoLetivo)
            //    .NotEmpty()
            //    .WithMessage("É necessário informar o ano letivo para inserir importação de arquivo.")
            //    .GreaterThanOrEqualTo(2019)
            //    .WithMessage("O ano letivo deve ser maior ou igual a 2019.");

            RuleFor(x => x.ImportacaoLog.TipoArquivoImportacao)
                .NotEmpty()
                .WithMessage("É necessário informar o tipo do arquivo para importação.");

            RuleFor(x => x.ImportacaoLog.StatusImportacao)
                .NotEmpty()
                .WithMessage("É necessário informar o status para importação.");
        }
    }
}
