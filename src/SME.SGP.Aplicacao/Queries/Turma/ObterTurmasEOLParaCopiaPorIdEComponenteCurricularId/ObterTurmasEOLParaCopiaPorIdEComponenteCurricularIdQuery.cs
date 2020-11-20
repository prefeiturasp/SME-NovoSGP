using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery : IRequest<IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery(int turmaId, long componenteCurricularId, string codigoRF)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            CodigoRF = codigoRF;
        }
        public long ComponenteCurricularId { get; set; }
        public int TurmaId { get; set; }
        public string CodigoRF { get; set; }

    }

    public class ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQueryValidator : AbstractValidator<ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery>
    {

        public ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado.");

            RuleFor(c => c.CodigoRF)
                .NotEmpty()
                .WithMessage("O Usuário deve ser informado");
        }
    }
}
