using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery : IRequest<IEnumerable<PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto>>
    {
        public ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery(int anoLetivo, ModalidadeTipoCalendario modalidade)
        {

            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
    }
}
