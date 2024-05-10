using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardAcompanhamentoAprendizagemPorDreQuery : IRequest<IEnumerable<DashboardAcompanhamentoAprendizagemPorDreDto>>
    {
        public ObterDashboardAcompanhamentoAprendizagemPorDreQuery(int anoLetivo, int? semestre)
        {
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public int AnoLetivo { get; }
        public int? Semestre { get; }
    }

    public class ObterDashboardAcompanhamentoAprendizagemPorDreQueryValidator : AbstractValidator<ObterDashboardAcompanhamentoAprendizagemPorDreQuery>
    {
        public ObterDashboardAcompanhamentoAprendizagemPorDreQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de total de acompanhamentos de aprendizagem por DRE");
        }
    }
}
