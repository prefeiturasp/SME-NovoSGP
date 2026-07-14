using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery : IRequest<IEnumerable<AulaPrevistaBimestre>>
    {
        public long AulaPrevistaId { get; set; }
        public int[] Bimestres { get; set; }

        public ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery(long aulaPrevistaId, int[] bimestres)
        {
            AulaPrevistaId = aulaPrevistaId;
            Bimestres = bimestres;
        }
    }

    public class ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQueryValidator : AbstractValidator<ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery>
    {
        public ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQueryValidator()
        {
            RuleFor(a => a.AulaPrevistaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da aula prevista para consultas de aula prevista por bimestre");

            RuleFor(a => a.Bimestres)
                .NotEmpty()
                .WithMessage("É necessário informar o(s) bimestre(s) para consultas das aulas previstas do bimestre");
        }
    }
}
