using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorTurmaEConsideraHistoricoQuery : IRequest<AbrangenciaFiltroRetorno>
    {
        public ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(string turmaId, bool consideraHistorico=false)
        {
            TurmaId = turmaId;
            ConsideraHistorico = consideraHistorico;
        }

        public string TurmaId { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
    public class ObterAbrangenciaPorTurmaEConsideraHistoricoQueryValidator : AbstractValidator<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery>
    {
        public ObterAbrangenciaPorTurmaEConsideraHistoricoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty().WithMessage("É necessário informar a Turma");
            RuleFor(a => a.ConsideraHistorico)
                .NotEmpty().WithMessage("É necessário informar se considera histórico ");
        }
    }
}