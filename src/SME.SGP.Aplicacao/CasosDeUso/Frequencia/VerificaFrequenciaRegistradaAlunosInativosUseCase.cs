using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaFrequenciaRegistradaAlunosInativosUseCase : AbstractUseCase, IVerificaFrequenciaRegistradaAlunosInativosUseCase
    {
        public VerificaFrequenciaRegistradaAlunosInativosUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            bool validaTurma = !String.IsNullOrEmpty(param.Mensagem.ToString());

            if (validaTurma)
                 return await mediator.Send(new VerificaFrequenciaRegistradaParaAlunosInativosCommand() { TurmaCodigo = param.Mensagem.ToString() });
            
            return false;
        }
    }
}
