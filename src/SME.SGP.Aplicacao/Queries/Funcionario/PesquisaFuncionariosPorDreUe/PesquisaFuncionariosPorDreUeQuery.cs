using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PesquisaFuncionariosPorDreUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public PesquisaFuncionariosPorDreUeQuery(string codigoRF, string codigoDRE, string codigoUE, string codigoTurma, string nome)
        {
            CodigoRF = codigoRF;
            CodigoDRE = codigoDRE;
            CodigoUE = codigoUE;
            CodigoTurma = codigoTurma;
            Nome = nome;
        }

        public string CodigoRF { get; }
        public string CodigoDRE { get; }
        public string CodigoUE { get; }
        public string CodigoTurma { get; }
        public string Nome { get; }
    }
}
