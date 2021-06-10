using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesAnosItineranciaProgramaQuery : IRequest<IEnumerable<ModalidadesPorAnoItineranciaProgramaDto>>
    {
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }

        public ObterModalidadesAnosItineranciaProgramaQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DreId = dreId;
            UeId = ueId;
            Semestre = semestre;
        }
    }

    public class ObterModalidadesAnosItineranciaProgramaQueryValidator : AbstractValidator<ObterModalidadesAnosItineranciaProgramaQuery>
    {
        public ObterModalidadesAnosItineranciaProgramaQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("É obrigatório informar os Ano Letivo");
        }
    }
}
