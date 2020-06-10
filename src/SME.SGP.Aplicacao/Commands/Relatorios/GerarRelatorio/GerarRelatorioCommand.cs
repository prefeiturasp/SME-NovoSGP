using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands.Relatorios.GerarRelatorio
{
    public class GerarRelatorioCommand: IRequest<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoRelatorio">Endpoint do relatório no servidor de relatórios, descrito na tag DisplayName</param>
        /// <param name="filtros">Classe de filtro vindo do front</param>
        public GerarRelatorioCommand(TipoRelatorio tipoRelatorio, object filtros)
        {
            TipoRelatorio = tipoRelatorio;
            Filtros = filtros;
        }

        /// <summary>
        /// Classe de filtro vindo do front
        /// </summary>
        public object Filtros { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
    }
}
