using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DiarioBordoObservacaoMap : BaseEntityMap<DiarioBordoObservacao>
    {
        public DiarioBordoObservacaoMap()
        {
            ToTable("diario_bordo_observacao");
            Map(nameof(DiarioBordoObservacao.Observacao), "observacao");
            Map(nameof(DiarioBordoObservacao.DiarioBordoId), "diario_bordo_id");
            Map(nameof(DiarioBordoObservacao.UsuarioId), "usuario_id");
            Map(nameof(DiarioBordoObservacao.Excluido), "excluido");
        }
    }
}