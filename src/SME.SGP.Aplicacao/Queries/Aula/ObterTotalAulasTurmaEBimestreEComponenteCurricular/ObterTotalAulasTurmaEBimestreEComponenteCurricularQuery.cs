using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery : IRequest<IEnumerable<TurmaComponenteQntAulasDto>>
    {
        public ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(string[] turmasCodigo, long tipoCalendarioId, string[] componentesCurricularesId, int[] bimestres)
        {
            TurmasCodigo = turmasCodigo;
            TipoCalendarioId = tipoCalendarioId;
            ComponentesCurricularesId = componentesCurricularesId;
            Bimestres = bimestres;
        }

        public string[] TurmasCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public int[] Bimestres { get; set; }

     
    }

    public class ObterTotalAulasTurmaEBimestreEComponenteCurricularQueryValidator : AbstractValidator<ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery>
    {
        public ObterTotalAulasTurmaEBimestreEComponenteCurricularQueryValidator()
        {
            RuleFor(x => x.ComponentesCurricularesId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(x => x.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(x => x.ComponentesCurricularesId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(x => x.Bimestres)
                .NotEmpty()
                .When(x => x.Bimestres != null)
                .WithMessage("O bimestre deve ser informado.");
        }
    }
}