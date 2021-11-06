using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterGestoresDreUePorTipoCalendarioModalidadeQuery : IRequest<IEnumerable<GestoresDreUePorTipoModalidadeCalendarioDto>>
    {
        public ObterGestoresDreUePorTipoCalendarioModalidadeQuery(ModalidadeTipoCalendario tipoCalendario, int anoLetivo)
        {
            TipoCalendario = tipoCalendario;
            AnoLetivo = anoLetivo;
        }

        public ModalidadeTipoCalendario TipoCalendario { get; set; }
        public int AnoLetivo { get; set; }
    }
}
