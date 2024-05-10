using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CicloEnsinoMap : BaseMap<CicloEnsino>
    {
        public CicloEnsinoMap()
        {
            ToTable("ciclo_ensino");
            Map(c => c.CodEol).ToColumn("cod_ciclo_ensino_eol");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DtAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.CodigoModalidadeEnsino).ToColumn("codigo_modalidade_ensino");
            Map(c => c.CodigoEtapaEnsino).ToColumn("codigo_etapa_ensino");
        }
    }
}