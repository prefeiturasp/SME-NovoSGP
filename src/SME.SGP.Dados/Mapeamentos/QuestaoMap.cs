using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class QuestaoMap : BaseEntityMap<Questao>
    {
        public QuestaoMap()
        {
            ToTable("questao");
            Map(nameof(Questao.QuestionarioId), "questionario_id");
            Map(nameof(Questao.Ordem), "ordem");
            Map(nameof(Questao.Nome), "nome");
            Map(nameof(Questao.Observacao), "observacao");
            Map(nameof(Questao.Obrigatorio), "obrigatorio");
            Map(nameof(Questao.Tipo), "tipo");
            Map(nameof(Questao.Opcionais), "opcionais");
            Map(nameof(Questao.SomenteLeitura), "somente_leitura");
            Map(nameof(Questao.Dimensao), "dimensao");
            Map(nameof(Questao.Tamanho), "tamanho");
            Map(nameof(Questao.Mascara), "mascara");
            Map(nameof(Questao.PlaceHolder), "placeholder");
            Map(nameof(Questao.NomeComponente), "nome_componente");
        }
    }
}