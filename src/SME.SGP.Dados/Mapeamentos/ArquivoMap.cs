using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoMap : BaseMap<Arquivo>
    {
        public ArquivoMap()
        {
            ToTable("arquivo");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.TipoConteudo).ToColumn("tipo_conteudo");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}
