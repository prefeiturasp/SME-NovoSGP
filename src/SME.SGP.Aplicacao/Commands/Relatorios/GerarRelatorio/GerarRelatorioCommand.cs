using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioCommand : IRequest<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoRelatorio">Endpoint do relatório no servidor de relatórios, descrito na tag DisplayName</param>
        /// <param name="filtros">Classe de filtro vindo do front</param>
        /// /// <param name="rotaRelatorio">Rota rabbit do relatório a ser gerado</param>
        public GerarRelatorioCommand(TipoRelatorio tipoRelatorio, object filtros, Usuario usuario, string rotaRelatorio = RotasRabbitSgpRelatorios.RotaRelatoriosSolicitados, TipoFormatoRelatorio formato  = TipoFormatoRelatorio.Pdf,bool notificarErroUsuario = false)
        {
            TipoRelatorio = tipoRelatorio;
            Filtros = filtros;
            IdUsuarioLogado = usuario.Id;
            UsuarioLogadoRf = usuario.CodigoRf;
            Formato = formato;
            PerfilUsuario = usuario.PerfilAtual.ToString();
            RotaRelatorio = rotaRelatorio;
            NotificarErroUsuario = notificarErroUsuario;
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
        public string RotaRelatorio { get; set; }
        public bool NotificarErroUsuario { get; set; }
    }
}
