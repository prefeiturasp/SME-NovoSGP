using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterPeriodoEscolaFechamentoReaberturaQuery : IRequest<(SME.SGP.Dominio.PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento, Dominio.Aplicacao)>
    {
        public ObterPeriodoEscolaFechamentoReaberturaQuery(long tipoCalendarioId, Ue ue, int bimestre, Dominio.Aplicacao aplicacao)
        {
            TipoCalendarioId = tipoCalendarioId;
            Ue = ue;
            Bimestre = bimestre;
            Aplicacao = aplicacao;
        }

        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public int Bimestre { get; set; }
        public Dominio.Aplicacao Aplicacao { get; set; }
    }
    public class ObterPeriodoEscolaFechamentoReaberturaQueryValidator : AbstractValidator<ObterPeriodoEscolaFechamentoReaberturaQuery>
    {
        public ObterPeriodoEscolaFechamentoReaberturaQueryValidator()
        {
            RuleFor(x => x.TipoCalendarioId).GreaterThan(0);
            RuleFor(x => x.Ue).NotNull();
            RuleFor(x => x.Bimestre).GreaterThan(0);
        }
    }
}