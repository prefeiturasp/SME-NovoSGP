using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class MapeamentoEstudanteSecaoMap : BaseMap<MapeamentoEstudanteSecao>
    {
        public MapeamentoEstudanteSecaoMap()
        {
            ToTable("mapeamento_estudante_secao");

            Map(nameof(MapeamentoEstudanteSecao.MapeamentoEstudanteId), "mapeamento_estudante_id");
            Map(nameof(MapeamentoEstudanteSecao.SecaoMapeamentoEstudanteId), "secao_mapeamento_estudante_id");
            Map(nameof(MapeamentoEstudanteSecao.Concluido), "concluido");
            Map(nameof(MapeamentoEstudanteSecao.Excluido), "excluido");
        }
    }
}