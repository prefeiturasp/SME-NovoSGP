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
        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(int anoLetivo, string dreCodigo, string ueCodigo, string turmaCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            AnoLetivo = anoLetivo;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            Modalidade = modalidade;
            Semestre = semestre;
            Mes = mes;
        }

        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Mes { get; set; }
    }

    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator : AbstractValidator<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>
    {
        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .NotNull()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}
