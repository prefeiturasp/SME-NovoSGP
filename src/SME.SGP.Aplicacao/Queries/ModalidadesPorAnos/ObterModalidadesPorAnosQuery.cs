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
        public int modalidade { get; set; }
        public int semestre { get; set; }

        public ObterModalidadesPorAnosQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            this.modalidade = modalidade;
            DreId = dreId;
            UeId = ueId;
            this.semestre = semestre;
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
