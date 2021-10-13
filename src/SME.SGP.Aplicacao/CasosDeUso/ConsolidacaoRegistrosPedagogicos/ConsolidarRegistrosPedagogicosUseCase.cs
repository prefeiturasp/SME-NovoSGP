using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ConsolidacaoRegistrosPedagogicos
{
    public class ConsolidarRegistrosPedagogicosUseCase : AbstractUseCase, IConsolidarRegistrosPedagogicosUseCase
    {
        private ParametrosSistema parametroConsolidacao;

        public ConsolidarRegistrosPedagogicosUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            return true;
        }
    }
}
