using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAulaMap : BaseEntityMap<PlanoAula>
    {
        public PlanoAulaMap()
        {
            ToTable("plano_aula");
            Map(nameof(PlanoAula.AulaId), "aula_id");
            Map(nameof(PlanoAula.Descricao), "descricao");
            Map(nameof(PlanoAula.RecuperacaoAula), "recuperacao_aula");
            Map(nameof(PlanoAula.LicaoCasa), "licao_casa");
            Map(nameof(PlanoAula.Migrado), "migrado");
            Map(nameof(PlanoAula.Excluido), "excluido");
        }
    }
}