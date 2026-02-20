using SME.SGP.Dominio.Enumerados;
using System.Text.Json;

namespace SME.SGP.Dominio.Entidades
{
    public class SolicitacaoRelatorio : EntidadeBase
    {
        public string FiltrosUsados { get; set; }
        public TipoFormatoRelatorio TipoRelatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
        public StatusSolicitacao StatusSolicitacao { get; set; }
        public bool Excluido { get; set; }


        public T ObterFiltros<T>() where T : FiltroRelatorioBase
        {
            return JsonSerializer.Deserialize<T>(FiltrosUsados);
        }
    }
}
