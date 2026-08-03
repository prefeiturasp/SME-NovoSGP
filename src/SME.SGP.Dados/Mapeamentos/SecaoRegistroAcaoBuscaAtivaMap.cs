using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class SecaoRegistroAcaoBuscaAtivaMap : BaseMap<SecaoRegistroAcaoBuscaAtiva>
    {
        public SecaoRegistroAcaoBuscaAtivaMap()
        {
            ToTable("secao_registro_acao_busca_ativa");

            Map(nameof(SecaoRegistroAcaoBuscaAtiva.QuestionarioId), "questionario_id");
            Map(nameof(SecaoRegistroAcaoBuscaAtiva.Nome), "nome");
            Map(nameof(SecaoRegistroAcaoBuscaAtiva.Ordem), "ordem");
            Map(nameof(SecaoRegistroAcaoBuscaAtiva.Etapa), "etapa");
            Map(nameof(SecaoRegistroAcaoBuscaAtiva.Excluido), "excluido");
            Map(nameof(SecaoRegistroAcaoBuscaAtiva.NomeComponente), "nome_componente");
        }
    }
}