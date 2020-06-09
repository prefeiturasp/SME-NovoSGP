using MediatR;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Commands
{
    public class GerarRelatorioComand : IRequest<bool>
    {
        public GerarRelatorioComand(TipoRelatorio tipoRelatorio, object filtros)
        {
            TipoRelatorio = tipoRelatorio;
            Filtros = filtros;
        }

        public object Filtros { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
    }
}
