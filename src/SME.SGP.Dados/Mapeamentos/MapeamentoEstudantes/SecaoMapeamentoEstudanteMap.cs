using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class SecaoMapeamentoEstudanteMap : BaseMap<SecaoMapeamentoEstudante>
    {
        public SecaoMapeamentoEstudanteMap()
        {
            ToTable("secao_mapeamento_estudante");

            Map(nameof(SecaoMapeamentoEstudante.QuestionarioId), "questionario_id");
            Map(nameof(SecaoMapeamentoEstudante.Nome), "nome");
            Map(nameof(SecaoMapeamentoEstudante.Ordem), "ordem");
            Map(nameof(SecaoMapeamentoEstudante.Etapa), "etapa");
            Map(nameof(SecaoMapeamentoEstudante.Excluido), "excluido");
            Map(nameof(SecaoMapeamentoEstudante.NomeComponente), "nome_componente");
        }
    }
}