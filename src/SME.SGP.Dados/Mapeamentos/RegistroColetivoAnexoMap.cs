using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoAnexoMap : BaseMap<RegistroColetivoAnexo>
    {
        public RegistroColetivoAnexoMap()
        {
            ToTable("registrocoletivo_anexo");
            Map(c => c.RegistroColetivoId).ToColumn("registrocoletivo_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
