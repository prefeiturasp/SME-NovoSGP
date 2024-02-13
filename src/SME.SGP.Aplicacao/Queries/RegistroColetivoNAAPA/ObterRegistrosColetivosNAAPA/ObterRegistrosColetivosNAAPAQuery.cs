using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosColetivosNAAPAQuery : IRequest<PaginacaoResultadoDto<RegistroColetivoListagemDto>>
    {
        public ObterRegistrosColetivosNAAPAQuery(FiltroRegistroColetivoDto filtro)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            DataReuniaoInicio = filtro.DataReuniaoInicio;
            DataReuniaoFim = filtro.DataReuniaoFim;
            TiposReuniaoId = filtro.TiposReuniaoId;
        }
        public long DreId { get; set; }
        public long? UeId { get; set; }
        public DateTime? DataReuniaoInicio { get; set; }
        public DateTime? DataReuniaoFim { get; set; }
        public long[] TiposReuniaoId { get; set; }
    }

    public class ObterRegistrosColetivosNAAPAQueryValidator : AbstractValidator<ObterRegistrosColetivosNAAPAQuery>
    {
        public ObterRegistrosColetivosNAAPAQueryValidator()
        {
            RuleFor(c => c.DreId).NotEmpty().WithMessage("O identificador da DRE deve ser informado para pesquisa de Registros Coletivos NAAPA");
        }
    }
}
