using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class HistoricoReinicioSenhaMap: BaseMap<HistoricoReinicioSenha>
    {
        public HistoricoReinicioSenhaMap()
        {
            ToTable("historico_reinicio_senha");
            Map(c => c.UsuarioRf).ToColumn("usuario_rf");
            Map(c => c.DreCodigo).ToColumn("dre_codigo");
            Map(c => c.UeCodigo).ToColumn("ue_codigo");
        }
    }
}
