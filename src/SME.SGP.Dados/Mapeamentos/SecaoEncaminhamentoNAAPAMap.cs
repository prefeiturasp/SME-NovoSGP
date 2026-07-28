using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class SecaoEncaminhamentoNAAPAMap : BaseEntityMap<SecaoEncaminhamentoNAAPA>
    {
        public SecaoEncaminhamentoNAAPAMap()
        {
            ToTable("secao_encaminhamento_naapa");

            Map(nameof(SecaoEncaminhamentoNAAPA.QuestionarioId), "questionario_id");
            Map(nameof(SecaoEncaminhamentoNAAPA.Nome), "nome");
            Map(nameof(SecaoEncaminhamentoNAAPA.Ordem), "ordem");
            Map(nameof(SecaoEncaminhamentoNAAPA.Etapa), "etapa");
            Map(nameof(SecaoEncaminhamentoNAAPA.Excluido), "excluido");
            Map(nameof(SecaoEncaminhamentoNAAPA.NomeComponente), "nome_componente");
        }
    }
}