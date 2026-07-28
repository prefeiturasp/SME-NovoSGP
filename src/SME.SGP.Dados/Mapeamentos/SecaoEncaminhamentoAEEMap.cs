using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class SecaoEncaminhamentoAEEMap : BaseEntityMap<SecaoEncaminhamentoAEE>
    {
        public SecaoEncaminhamentoAEEMap()
        {
            ToTable("secao_encaminhamento_aee");

            Map(nameof(SecaoEncaminhamentoAEE.QuestionarioId), "questionario_id");
            Map(nameof(SecaoEncaminhamentoAEE.Nome), "nome");
            Map(nameof(SecaoEncaminhamentoAEE.Ordem), "ordem");
            Map(nameof(SecaoEncaminhamentoAEE.Etapa), "etapa");
            Map(nameof(SecaoEncaminhamentoAEE.Excluido), "excluido");
            Map(nameof(SecaoEncaminhamentoAEE.NomeComponente), "nome_componente");
        }
    }
}