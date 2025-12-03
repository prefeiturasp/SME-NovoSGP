using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA
{
    public interface IObterSecoesAtendimentoIndividualNAAPAUseCase
    {
        Task<IEnumerable<SecaoQuestionarioDto>> Executar(long? EncaminhamentoNAAPAId);
    }
}
