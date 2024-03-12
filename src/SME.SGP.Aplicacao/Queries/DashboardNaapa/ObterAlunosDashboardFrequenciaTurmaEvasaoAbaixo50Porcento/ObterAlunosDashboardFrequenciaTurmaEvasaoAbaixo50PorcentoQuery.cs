using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery : IRequest<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, int mes)
        {
            FiltroAbrangencia = filtroAbrangencia;
            Mes = mes;
        }

        public FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto FiltroAbrangencia { get; set; }
        public int Mes { get; set; }
    }

    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator : AbstractValidator<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>
    {
        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator()
        {
            RuleFor(c => c.FiltroAbrangencia)
                .NotNull()
                .WithMessage("O filtro de abrangência deve ser informado.");

            RuleFor(c => c.FiltroAbrangencia.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.FiltroAbrangencia.Modalidade)
                .NotEmpty()
                .NotNull()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}
