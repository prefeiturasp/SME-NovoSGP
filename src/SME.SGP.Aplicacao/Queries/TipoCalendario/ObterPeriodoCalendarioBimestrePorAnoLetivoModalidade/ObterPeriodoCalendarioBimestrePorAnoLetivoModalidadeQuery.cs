using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery : IRequest<IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>>
    {
        public ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {

            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQueryValidator : AbstractValidator<ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery>
    {
        public ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("Ano Letivo deve ser preenchido para obtenção de Período Escolar e Bimestre.");
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("Modalidade do Tipo Calendário deve ser preenchida para obtenção de Período Escolar e Bimestre.");
        }
    }
}
