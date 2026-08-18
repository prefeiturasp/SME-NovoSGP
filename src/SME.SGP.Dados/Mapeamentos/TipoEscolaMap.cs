using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoEscolaMap : BaseMap<TipoEscolaEol>
    {
        public TipoEscolaMap()
        {
            ToTable("tipo_escola");

            Map(nameof(TipoEscolaEol.CodEol), "cod_tipo_escola_eol");
            Map(nameof(TipoEscolaEol.Descricao), "descricao");
            Map(nameof(TipoEscolaEol.DtAtualizacao), "data_atualizacao");
        }
    }
}