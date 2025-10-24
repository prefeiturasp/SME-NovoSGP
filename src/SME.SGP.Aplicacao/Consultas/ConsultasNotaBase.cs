using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasNotaBase
    {
        private readonly IContextoAplicacao contextoAplicacao;

        protected ConsultasNotaBase(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = contextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = contextoAplicacao.ObterVariavel<string>("NumeroRegistros");

                if (string.IsNullOrWhiteSpace(numeroPaginaQueryString) || string.IsNullOrWhiteSpace(numeroRegistrosQueryString))
                    return new Paginacao(0, 0);

                var numeroPagina = int.Parse(numeroPaginaQueryString);
                var numeroRegistros = int.Parse(numeroRegistrosQueryString);

                return new Paginacao(numeroPagina, numeroRegistros);
            }
        }
    }
}
