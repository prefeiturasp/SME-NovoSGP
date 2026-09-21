using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoAnexoMap : SimpleMap<InformativoAnexo>
    {
        public InformativoAnexoMap()
        {
            ToTable("informativo_anexo");
            Map(nameof(InformativoAnexo.InformativoId), "informativo_id");
            Map(nameof(InformativoAnexo.ArquivoId), "arquivo_id");
        }
    }
}