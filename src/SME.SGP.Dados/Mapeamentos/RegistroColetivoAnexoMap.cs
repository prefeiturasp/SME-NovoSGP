using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoAnexoMap : BaseEntityMap<RegistroColetivoAnexo>
    {
        public RegistroColetivoAnexoMap()
        {
            ToTable("registrocoletivo_anexo");
            Map(nameof(RegistroColetivoAnexo.RegistroColetivoId), "registrocoletivo_id");
            Map(nameof(RegistroColetivoAnexo.ArquivoId), "arquivo_id");
            Map(nameof(RegistroColetivoAnexo.Excluido), "excluido");
        }
    }
}