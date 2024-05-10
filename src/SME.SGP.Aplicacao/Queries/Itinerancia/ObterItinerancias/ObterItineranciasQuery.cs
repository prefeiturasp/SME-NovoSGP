using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasQuery : IRequest<PaginacaoResultadoDto<ItineranciaRetornoQueryDto>>
    {
        public ObterItineranciasQuery(FiltroPesquisaItineranciasDto filtro)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            TurmaId = filtro.TurmaId;
            AnoLetivo = filtro.AnoLetivo;
            AlunoCodigo = filtro.AlunoCodigo;
            DataInicio = filtro.DataInicio;
            DataFim = filtro.DataFim;
            Situacao = filtro.Situacao;
            CriadoRf = filtro.CriadoRf;
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
