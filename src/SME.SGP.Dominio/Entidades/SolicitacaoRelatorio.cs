using SME.SGP.Dominio.Enumerados;
using System;
using System.Text.Json;

namespace SME.SGP.Dominio.Entidades
{
    public class SolicitacaoRelatorio : EntidadeBase
    {
        public SolicitacaoRelatorio()
        {
            SolicitadoEm = DateTimeExtension.HorarioBrasilia();
        }

        public string FiltrosUsados { get; set; }
        public TipoFormatoRelatorio ExtensaoRelatorio { get; set; }
        public TipoRelatorio Relatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
        public StatusSolicitacao StatusSolicitacao { get; set; }
        public bool Excluido { get; set; }
        public DateTime SolicitadoEm { get; set; }


        public T ObterFiltros<T>() where T : FiltroRelatorioBase
        {
            return JsonSerializer.Deserialize<T>(FiltrosUsados);
        }
    }
}
