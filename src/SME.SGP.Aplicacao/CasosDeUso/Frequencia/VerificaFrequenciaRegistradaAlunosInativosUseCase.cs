using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaFrequenciaRegistradaAlunosInativosUseCase : IVerificaFrequenciaRegistradaAlunosInativosUseCase
    {
        public Task<bool> Executar(MensagemRabbit param)
        {
            bool validaTurma = !String.IsNullOrEmpty(param.Mensagem.ToString());

            if (validaTurma)
            {

            }
            return null;
        }
    }
}
