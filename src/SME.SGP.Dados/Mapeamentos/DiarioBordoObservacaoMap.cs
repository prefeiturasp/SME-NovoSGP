using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DiarioBordoObservacaoMap : BaseMap<DiarioBordoObservacao>
    {
        public DiarioBordoObservacaoMap()
        {
            ToTable("diario_bordo_observacao");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.DiarioBordoId).ToColumn("diario_bordo_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}