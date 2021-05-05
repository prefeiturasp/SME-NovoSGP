using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorDreQuery : IRequest<IEnumerable<GraficoFrequenciaGlobalPorDREDto>>
    {
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public int? Semestre { get; set; }

        public ObterDadosDashboardFrequenciaPorDreQuery(int anoLetivo, Modalidade modalidade, string ano, int? semestre)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            Ano = ano;
            Semestre = semestre;
        }
    }

    public class ObterDadosDashboardFrequenciaPorDreQueryValidator : AbstractValidator<ObterDadosDashboardFrequenciaPorDreQuery>
    {
        public ObterDadosDashboardFrequenciaPorDreQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a busca de frequência global por DRE.");

            RuleFor(x => x.Ano)
                .Matches("^[1-9]{1}")
                .When(x => !string.IsNullOrWhiteSpace(x.Ano))
                .WithMessage("O ano escolar infomado é inválido.");

            RuleFor(x => x.Semestre)
                .NotEmpty()
                .When(x => x.Modalidade == Modalidade.EJA)
                .WithMessage("Para a modalidade EJA o semestre deve ser informado.")
                .InclusiveBetween(1, 2)
                .WithMessage("O semestre informado é inválido.");
        }
    }
}