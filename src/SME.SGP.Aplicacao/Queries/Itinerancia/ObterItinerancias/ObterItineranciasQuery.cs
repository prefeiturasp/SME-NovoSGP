using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasQuery : IRequest<PaginacaoResultadoDto<ItineranciaRetornoQueryDto>>
    {
        public ObterItineranciasQuery(long dreId, long ueId, long turmaId, int anoLetivo, string alunoCodigo, DateTime? dataInicio, DateTime? dataFim, SituacaoItinerancia situacao, string criadoRf)
        {
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            AlunoCodigo = alunoCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Situacao = situacao;
            CriadoRf = criadoRf;
        }

        public long DreId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public string AlunoCodigo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public SituacaoItinerancia Situacao { get; set; }
        public string CriadoRf { get; set; }
    }

    public class ObterItineranciasQueryValidator : AbstractValidator<ObterItineranciasQuery>
    {
        public ObterItineranciasQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa dos registros de itinerância");
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado para pesquisa dos registros de itinerância");
        }
    }
}
