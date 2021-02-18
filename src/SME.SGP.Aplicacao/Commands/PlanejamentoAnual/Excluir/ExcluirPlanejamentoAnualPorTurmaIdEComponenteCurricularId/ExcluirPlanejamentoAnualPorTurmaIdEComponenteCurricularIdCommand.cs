using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand : IRequest<bool>
    {
        public ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand()
        {

        }
        public ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand(long turmaId, long componenteCurricularId, long[] idsPlanejamentoAnualPeriodoEscolar)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            IdsPlanejamentoAnualPeriodoEscolar = idsPlanejamentoAnualPeriodoEscolar;
        }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long[] IdsPlanejamentoAnualPeriodoEscolar { get; set; }
    }


    public class RemovePlanejamentoAnualAntigoCommandValidator : AbstractValidator<ExcluirPlanejamentoAnualPorTurmaIdEComponenteCurricularIdCommand>
    {
        public RemovePlanejamentoAnualAntigoCommandValidator()
        {

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");
        }
    }
}
