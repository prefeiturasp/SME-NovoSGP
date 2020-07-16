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
            Map(a => a.UsuarioRf).ToColumn("usuario_rf");
            Map(a => a.DreCodigo).ToColumn("dre_codigo");
            Map(a => a.UeCodigo).ToColumn("ue_codigo");
        }
    }
}
