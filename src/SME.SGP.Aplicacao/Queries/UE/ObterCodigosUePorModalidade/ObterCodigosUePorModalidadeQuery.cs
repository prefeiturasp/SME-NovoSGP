using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosUePorModalidadeQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigosUePorModalidadeQuery(string[] codigosUe, Modalidade[] modalidades)
        {
            CodigosUe = codigosUe;
            Modalidades = modalidades;
        }
        public string[] CodigosUe { get; set; }
        public Modalidade[] Modalidades { get; set; }
    }
}
