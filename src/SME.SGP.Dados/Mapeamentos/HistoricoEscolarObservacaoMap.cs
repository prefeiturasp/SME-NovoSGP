using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoEscolarObservacaoMap : BaseMap<HistoricoEscolarObservacao>
    {
        public HistoricoEscolarObservacaoMap()
        {
            ToTable("historico_escolar_observacao");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.Observacao).ToColumn("observacao");
        }
    }
}

