using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery : IRequest<GraficoAtendimentoNAAPADto>
    {
        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery(int anoLetivo, long dreId, long? ueId, int? mes, Modalidade? modalidade)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Mes = mes;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long? UeId { get; set; }
        public int? Mes { get; set; }
        public Modalidade? Modalidade { get; set; }
    }

    public class ObterQuantidadeEncaminhamentoNAAPAPorProfissionalMesQueryValidator : AbstractValidator<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery>
    {
        public ObterQuantidadeEncaminhamentoNAAPAPorProfissionalMesQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.DreId)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da dre deve ser informado.");
        }
    }
}
