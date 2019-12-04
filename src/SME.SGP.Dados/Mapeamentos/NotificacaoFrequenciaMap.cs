using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoFrequenciaMap : BaseMap<NotificacaoFrequencia>
    {
        public NotificacaoFrequenciaMap()
        {
            ToTable("notificacao_frequencia");
            Map(c => c.NotificacaoCodigo).ToColumn("notificacao_codigo");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.AulaId).ToColumn("aula_id");
        }
    }
}
