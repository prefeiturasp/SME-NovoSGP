using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaTurmaComponenteSemAulasUseCase : IPendenciaTurmaComponenteSemAulasUseCase
    {
        public Task<bool> Executar(MensagemRabbit param)
        {
            throw new NotImplementedException();
        }
    }
}
