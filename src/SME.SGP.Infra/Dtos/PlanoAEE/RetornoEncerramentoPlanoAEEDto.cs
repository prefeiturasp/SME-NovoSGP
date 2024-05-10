using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class RetornoEncerramentoPlanoAEEDto
    {
        public long PlanoId { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string SituacaoDescricao { get; set; }


        public RetornoEncerramentoPlanoAEEDto(long planoId, SituacaoPlanoAEE situacao)
        {
            PlanoId = planoId;
            Situacao = situacao;
            SituacaoDescricao = situacao.Name(); 
        }
    }

}
