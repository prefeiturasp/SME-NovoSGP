using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PesquisaFuncionariosPorDreUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public PesquisaFuncionariosPorDreUeQuery(string codigoRF, string nome, string codigoDRE, string codigoUE = "", string codigoTurma = "", Usuario usuario = null)
        {
            CodigoRF = codigoRF;
            CodigoDRE = codigoDRE;
            CodigoUE = codigoUE;
            CodigoTurma = codigoTurma;
            Nome = nome;
            Usuario = usuario;
        }

        public string CodigoRF { get; }
        public string CodigoDRE { get; }
        public string CodigoUE { get; }
        public string CodigoTurma { get; }
        public string Nome { get; }
        public Usuario Usuario { get; }
    }
}
