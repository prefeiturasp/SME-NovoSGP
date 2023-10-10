using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoRelatorioPapPorAnoEletivoUseCase : IExecutarMigracaoRelatorioPapPorAnoEletivoUseCase
    {
        public Task<bool> Executar(MensagemRabbit param)
        {
            var anoSemestre = param.ObterObjetoMensagem<AnoSemestreDto>();

            throw new NotImplementedException();
        }
    }
}
