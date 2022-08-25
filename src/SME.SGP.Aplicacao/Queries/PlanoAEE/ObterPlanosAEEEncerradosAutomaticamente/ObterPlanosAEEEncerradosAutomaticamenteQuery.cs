using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEEncerradosAutomaticamenteQuery : IRequest<IEnumerable<PlanoAEE>>
    {
        public ObterPlanosAEEEncerradosAutomaticamenteQuery(int pagina, int quantidadeRegistrosPorPagina = 100)
        {
            Pagina = pagina;
            QuantidadeRegistrosPorPagina = quantidadeRegistrosPorPagina;
        }

        public int Pagina { get; set; }
        public int QuantidadeRegistrosPorPagina { get; set; }
    }
}
