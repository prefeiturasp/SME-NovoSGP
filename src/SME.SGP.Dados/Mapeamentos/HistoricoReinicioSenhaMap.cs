using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class HistoricoReinicioSenhaMap : BaseMap<HistoricoReinicioSenha>
    {
        public HistoricoReinicioSenhaMap()
        {
            ToTable("historico_reinicio_senha");
            Map(nameof(HistoricoReinicioSenha.UsuarioRf), "usuario_rf");
            Map(nameof(HistoricoReinicioSenha.DreCodigo), "dre_codigo");
            Map(nameof(HistoricoReinicioSenha.UeCodigo), "ue_codigo");
        }
    }
}