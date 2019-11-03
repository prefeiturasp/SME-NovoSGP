using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaMap : BaseMap<Aula>
    {
        public AulaMap()
        {
            ToTable("aula");
            Map(a => a.TipoAula).ToColumn("tipo_aula");
            Map(a => a.DisciplinaId).ToColumn("disciplina_id");
            Map(a => a.Quantidade).ToColumn("quantidade");
            Map(a => a.Data).ToColumn("data");
            Map(a => a.RecorrenciaAula).ToColumn("recorrencia_aula");
            Map(a => a.Excluido).ToColumn("excluido");
        }
    }
}
