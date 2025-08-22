using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarArquivoIdebCommand : IRequest<AuditoriaDto>
    {
        public SalvarArquivoIdebCommand(ArquivoIdebDto arquivoDto)
        {
            ArquivoIdeb = arquivoDto;
        }
        public ArquivoIdebDto ArquivoIdeb { get; }
    }

    public class ImportarArquivoIdebCommandValidator : AbstractValidator<SalvarArquivoIdebCommand>
    {
        public ImportarArquivoIdebCommandValidator()
        {
            var serieAnosPermitidos = Enum.GetValues(typeof(SerieAnoArquivoIdebIdepEnum))
                .Cast<SerieAnoArquivoIdebIdepEnum>()
                .ToDictionary(e => (int)e, e => e.GetEnumDisplayName());

            RuleFor(x => x.ArquivoIdeb.SerieAno)
                .Must(valor => serieAnosPermitidos.ContainsKey(valor))
                .WithMessage($"O tipo de Série/Ano informado é inválido. Valores permitidos: {string.Join(", ", serieAnosPermitidos)}");

            RuleFor(x => x.ArquivoIdeb.Nota)
                .NotNull().WithMessage("A nota deve ser informada.")
                .Must(nota => Math.Round(nota, 2) == nota)
                .WithMessage("A nota deve ser um decimal com no máximo 2 casas decimais.");
        }
    }
}
