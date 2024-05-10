using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoCPCommand : IRequest<bool>
    {
        public ExecutarVerificacaoPendenciaAvaliacaoCPCommand(int diasParaGeracaoDePendencia)
        {
            DiasParaGeracaoDePendencia = diasParaGeracaoDePendencia;
        }

        public int DiasParaGeracaoDePendencia { get; set; }
    }
}
