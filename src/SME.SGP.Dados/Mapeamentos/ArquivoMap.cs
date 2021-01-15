using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoMap : BaseMap<Arquivo>
    {
        public ArquivoMap()
        {
            ToTable("arquivo");
            Map(a => a.TipoConteudo).ToColumn("tipo_conteudo");
        }
    }
}
