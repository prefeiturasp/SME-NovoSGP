using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterSemestresComReaberturaAtivaPAPQuery : IRequest<IEnumerable<SemestreAcompanhamentoDto>>
    {
        public DateTime DataReferencia { get; set; }
        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
        public List<SemestreAcompanhamentoDto> Semestres { get; set; }

        public ObterSemestresComReaberturaAtivaPAPQuery(DateTime dataReferencia, long tipoCalendarioId, long ueId, List<SemestreAcompanhamentoDto> semestres)
        {
            DataReferencia = dataReferencia;
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
            Semestres = semestres;
        }
    }

    public class ObterSemestresComReaberturaAtivaPAPQueryValidator : AbstractValidator<ObterSemestresComReaberturaAtivaPAPQuery>
    {
        public ObterSemestresComReaberturaAtivaPAPQueryValidator()
        {
            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("É necessário informar uma data de referência para obter os semestres");

            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("É necessário informar o tipo de calendário para consultar os semestres.");

            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da UE para consultar os semestres.");
        }
    }
}
