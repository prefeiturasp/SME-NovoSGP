using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioCommand : IRequest<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoRelatorio">Endpoint do relatório no servidor de relatórios, descrito na tag DisplayName</param>
        /// <param name="filtros">Classe de filtro vindo do front</param>
        public GerarRelatorioCommand(TipoRelatorio tipoRelatorio, object filtros, Usuario usuario, TipoFormatoRelatorio formato  = TipoFormatoRelatorio.Pdf)
        {
            TipoRelatorio = tipoRelatorio;
            Filtros = filtros;
            IdUsuarioLogado = usuario.Id;
            UsuarioLogadoRf = usuario.CodigoRf;
            Formato = formato;
            PerfilUsuario = usuario.PerfilAtual.ToString();
        }

        /// <summary>
        /// Classe de filtro vindo do front
        /// </summary>
        public object Filtros { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
        public long IdUsuarioLogado { get; set; }
        public string UsuarioLogadoRf { get; }
        public TipoFormatoRelatorio Formato { get; set; }
        public string PerfilUsuario { get; }
    }
}
