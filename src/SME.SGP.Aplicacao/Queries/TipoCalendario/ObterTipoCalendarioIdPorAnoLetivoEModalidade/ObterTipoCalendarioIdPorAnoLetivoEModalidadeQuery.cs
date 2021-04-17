using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery : IRequest<long>
    {
        public ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(ModalidadeTipoCalendario modalidade, int anoLetivo, int semestre)
        {
            Modalidade = modalidade;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public ModalidadeTipoCalendario Modalidade { get; }
        public int AnoLetivo { get; }
        public int Semestre { get; }
    }

    public class ObterTipoCalendarioIdPorAnoLetivoEModalidadeQueryValidator : AbstractValidator<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery>
    {
        public ObterTipoCalendarioIdPorAnoLetivoEModalidadeQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulto do tipo de calendário");
        }
    }
}
