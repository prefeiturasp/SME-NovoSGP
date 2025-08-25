using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora
{
    public class SalvarImportacaoArquivoFluenciaLeitoraCommand : IRequest<ArquivoFluenciaLeitora>
    {
        public SalvarImportacaoArquivoFluenciaLeitoraCommand(ArquivoFluenciaLeitoraDto arquivoFluenciaLeitora)
        {
            ArquivoFluenciaLeitora = arquivoFluenciaLeitora;
        }
        public ArquivoFluenciaLeitoraDto ArquivoFluenciaLeitora { get; }
    }

    public class ImportarArquivoFluenciaLeitoraCommandValidator : AbstractValidator<SalvarImportacaoArquivoFluenciaLeitoraCommand>
    {
        public ImportarArquivoFluenciaLeitoraCommandValidator()
        {
            RuleFor(x => x.ArquivoFluenciaLeitora.AnoLetivo)
                .NotNull().WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoFluenciaLeitora.CodigoEOLTurma)
                .NotNull().WithMessage("Código EOL da Turma inválido");

            RuleFor(x => x.ArquivoFluenciaLeitora.CodigoEOLAluno)
                .NotNull().WithMessage("Código EOL do Aluno inválido");

            RuleFor(x => x.ArquivoFluenciaLeitora.Fluencia)
                .NotNull().WithMessage("Fluência inválida");

            RuleFor(x => x.ArquivoFluenciaLeitora.Periodo)
                .NotNull().WithMessage("Período inválido");
        }
    }
}
