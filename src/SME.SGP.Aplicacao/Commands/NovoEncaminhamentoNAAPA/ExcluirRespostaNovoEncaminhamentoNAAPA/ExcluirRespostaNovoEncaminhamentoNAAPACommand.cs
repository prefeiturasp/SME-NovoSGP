using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaNovoEncaminhamentoNAAPA
{
    public class ExcluirRespostaNovoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirRespostaNovoEncaminhamentoNAAPACommand(RespostaEncaminhamentoEscolar resposta)
        {
            Resposta = resposta;
        }

        public RespostaEncaminhamentoEscolar Resposta { get; }
    }
}