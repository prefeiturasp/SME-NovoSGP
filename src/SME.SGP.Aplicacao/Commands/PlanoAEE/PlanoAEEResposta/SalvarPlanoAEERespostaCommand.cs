using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEERespostaCommand : IRequest<long>
    {
        public long PlanoId { get; set; }
        public long PlanoAEEQuestaoId { get; set; }
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }

        public SalvarPlanoAEERespostaCommand(long planoId, long planoAEEQuestaoId, string resposta, TipoQuestao tipoQuestao)
        {
            PlanoId = planoId;
            PlanoAEEQuestaoId = planoAEEQuestaoId;
            Resposta = resposta;
            TipoQuestao = tipoQuestao;
        }
    }
}
