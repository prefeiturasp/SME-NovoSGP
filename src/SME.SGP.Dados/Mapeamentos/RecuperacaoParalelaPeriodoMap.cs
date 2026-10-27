using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaPeriodoMap : BaseMap<RecuperacaoParalelaPeriodo>
    {
        public RecuperacaoParalelaPeriodoMap()
        {
            ToTable("recuperacao_paralela_periodo");
            Map(nameof(RecuperacaoParalelaPeriodo.Descricao), "descricao");
            Map(nameof(RecuperacaoParalelaPeriodo.Excluido), "excluido");
            Map(nameof(RecuperacaoParalelaPeriodo.Nome), "nome");
            Map(nameof(RecuperacaoParalelaPeriodo.BimestreEdicao), "bimestre_edicao");
        }
    }
}