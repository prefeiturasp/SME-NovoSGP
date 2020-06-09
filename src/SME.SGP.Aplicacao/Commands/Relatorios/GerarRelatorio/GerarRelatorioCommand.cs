using MediatR;
using SME.SGP.Infra.Enumerados;

namespace SME.SGP.Aplicacao.Commands
{
    public class GerarRelatorioCommand : IRequest<bool>
    {
        public GerarRelatorioCommand(TipoRelatorio tipoRelatorio, object filtros)
        {
            TipoRelatorio = tipoRelatorio;
            Filtros = filtros;
        }

        public object Filtros { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
    }
}
