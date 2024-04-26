using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class RespostaQuestaoMapeamentoEstudanteDto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }
        public long? RespostaId { get; set; }
        public string Texto { get; set; }
        public Arquivo Arquivo { get; set; }
    }

    public static class ClassExtensions
    {
        public static MapeamentoEstudanteSecaoQuestaoDto ToMapeamentoEstudanteSecaoQuestaoDto(this RespostaQuestaoMapeamentoEstudanteDto source, 
                                                                                              TipoQuestao tipoQuestao,
                                                                                              long respostaMapeamentoEstudanteId)
        => new MapeamentoEstudanteSecaoQuestaoDto()
        {
            QuestaoId = source.QuestaoId,
            Resposta = source.RespostaId.HasValue ? source.RespostaId.ToString() : source.Texto,
            TipoQuestao = tipoQuestao,
            RespostaMapeamentoEstudanteId = respostaMapeamentoEstudanteId
        };
    }
}
