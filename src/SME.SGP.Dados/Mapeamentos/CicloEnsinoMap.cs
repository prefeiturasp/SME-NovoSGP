using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CicloEnsinoMap : BaseMap<CicloEnsino>
    {
        public CicloEnsinoMap()
        {
            ToTable("ciclo_ensino");
            Map(nameof(CicloEnsino.CodEol), "cod_ciclo_ensino_eol");
            Map(nameof(CicloEnsino.Descricao), "descricao");
            Map(nameof(CicloEnsino.DtAtualizacao), "data_atualizacao");
            Map(nameof(CicloEnsino.CodigoModalidadeEnsino), "codigo_modalidade_ensino");
            Map(nameof(CicloEnsino.CodigoEtapaEnsino), "codigo_etapa_ensino");
        }
    }
}