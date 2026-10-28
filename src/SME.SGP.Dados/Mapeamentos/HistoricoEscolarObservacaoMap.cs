using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoEscolarObservacaoMap : BaseMap<HistoricoEscolarObservacao>
    {
        public HistoricoEscolarObservacaoMap()
        {
            ToTable("historico_escolar_observacao");
            Map(nameof(HistoricoEscolarObservacao.AlunoCodigo), "aluno_codigo");
            Map(nameof(HistoricoEscolarObservacao.Observacao), "observacao");
        }
    }
}