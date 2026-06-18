using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioBordoQuery : IRequest<IEnumerable<AulaComComponenteDto>>
    {
        public string TurmaId { get; set; }
        public long[] ComponentesCurricularesId { get; set; }

        public ObterPendenciasDiarioBordoQuery(string turmaId, long[] componentesCurricularesId)
        {
            TurmaId = turmaId;
            ComponentesCurricularesId = componentesCurricularesId;
        }
    }

    public class ObterPendenciasDiarioBordoQueryValidator : AbstractValidator<ObterPendenciasDiarioBordoQuery>
    {
        public ObterPendenciasDiarioBordoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma para obter as pendências de diário de bordo");
        }
    }
}
