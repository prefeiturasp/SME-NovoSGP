using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoMap : BaseMap<Arquivo>
    {
        public ArquivoMap()
        {
            ToTable("arquivo");
            Map(nameof(Arquivo.Nome), "nome");
            Map(nameof(Arquivo.Codigo), "codigo");
            Map(nameof(Arquivo.TipoConteudo), "tipo_conteudo");
            Map(nameof(Arquivo.Tipo), "tipo");
        }
    }
}