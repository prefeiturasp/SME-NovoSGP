using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisASPPPorDreUseCase : IRemoverAtribuicaoResponsaveisASPPPorDreUseCase
    {
        public Task<bool> Executar(MensagemRabbit param)
        {
            throw new NotImplementedException();
        }
    }
}
