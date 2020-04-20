using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoEscolaMap : BaseMap<TipoEscolaEol>
    {
        public TipoEscolaMap()
        {
            ToTable("tipo_escola");
            Map(c => c.DtAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.CodEol).ToColumn("cod_tipo_escola_eol");
        }
    }
}