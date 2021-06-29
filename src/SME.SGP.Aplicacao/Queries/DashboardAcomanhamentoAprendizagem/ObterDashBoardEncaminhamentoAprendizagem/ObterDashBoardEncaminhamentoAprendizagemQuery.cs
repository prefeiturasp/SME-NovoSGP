using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardEncaminhamentoAprendizagemQuery : IRequest<IEnumerable<DashboardAcompanhamentoAprendizagemDto>>
    {
        public ObterDashBoardEncaminhamentoAprendizagemQuery(int anoLetivo, long dreId, long ueId, int semestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Semestre = semestre;
        }

        public int AnoLetivo { get; }
        public long DreId { get; }
        public long UeId { get; }
        public int Semestre { get; }
    }

    public class ObterDashBoardEncaminhamentoAprendizagemQueryValidator : AbstractValidator<ObterDashBoardEncaminhamentoAprendizagemQuery>
    {
        public ObterDashBoardEncaminhamentoAprendizagemQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta do Dashboard de Acompanhamento de Aprendizagem");
            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado para consulta do Dashboard de Acompanhamento de Aprendizagem");
        }
    }
}
