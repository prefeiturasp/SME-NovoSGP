using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoMap : BaseMap<RegistroColetivo>
    {
        public RegistroColetivoMap()
        {
            ToTable("registrocoletivo");
            Map(nameof(RegistroColetivo.DreId), "dre_id");
            Map(nameof(RegistroColetivo.TipoReuniaoId), "tipo_reuniao_id");
            Map(nameof(RegistroColetivo.DataRegistro), "data_registro");
            Map(nameof(RegistroColetivo.QuantidadeParticipantes), "quantidade_participantes");
            Map(nameof(RegistroColetivo.QuantidadeEducadores), "quantidade_educadores");
            Map(nameof(RegistroColetivo.QuantidadeEducandos), "quantidade_educandos");
            Map(nameof(RegistroColetivo.QuantidadeCuidadores), "quantidade_cuidadores");
            Map(nameof(RegistroColetivo.Descricao), "descricao");
            Map(nameof(RegistroColetivo.Observacao), "observacao");
            Map(nameof(RegistroColetivo.Excluido), "excluido");
        }
    }
}