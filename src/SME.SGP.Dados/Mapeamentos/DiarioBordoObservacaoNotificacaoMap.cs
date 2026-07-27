using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DiarioBordoObservacaoNotificacaoMap : SimpleEntityMap<DiarioBordoObservacaoNotificacao>
    {
        public DiarioBordoObservacaoNotificacaoMap()
        {
            ToTable("diario_bordo_observacao_notificacao");
            Map(nameof(DiarioBordoObservacaoNotificacao.IdObservacao), "observacao_id");
            Map(nameof(DiarioBordoObservacaoNotificacao.IdNotificacao), "notificacao_id");
        }
    }
}