using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnosQuery : IRequest<IEnumerable<ModalidadesPorAnoDto>>
    {
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public int AnoTurma { get; set; }

        public ObterModalidadesPorAnosQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DreId = dreId;
            UeId = ueId;
            Semestre = semestre;
        }
    }

    public class ObterModalidadesPorAnosQueryValidator : AbstractValidator<ObterModalidadesPorAnosQuery>
    {
        public ObterModalidadesPorAnosQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("É obrigatório informar os Ano Letivo");
        }
    }
}
