using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroPoaMap : BaseMap<RegistroPoa>
    {
        public RegistroPoaMap()
        {
            ToTable("registro_poa");
            Map(nameof(RegistroPoa.AnoLetivo), "ano_letivo");
            Map(nameof(RegistroPoa.Bimestre), "bimestre");
            Map(nameof(RegistroPoa.CodigoRf), "codigo_rf");
            Map(nameof(RegistroPoa.Descricao), "descricao");
            Map(nameof(RegistroPoa.DreId), "dre_id");
            Map(nameof(RegistroPoa.Excluido), "excluido");
            Map(nameof(RegistroPoa.Titulo), "titulo");
            Map(nameof(RegistroPoa.UeId), "ue_id");
        }
    }
}