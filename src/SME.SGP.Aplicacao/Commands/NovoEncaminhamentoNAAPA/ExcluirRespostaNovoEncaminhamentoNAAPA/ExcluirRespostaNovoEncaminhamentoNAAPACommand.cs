using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaNovoEncaminhamentoNAAPA
{
    public class ExcluirRespostaNovoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirRespostaNovoEncaminhamentoNAAPACommand(RespostaEncaminhamentoNAAPA resposta)
        {
            Resposta = resposta;
        }

        public RespostaEncaminhamentoNAAPA Resposta { get; }
    }
}