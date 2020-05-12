using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresRelatorioPAPQuery : IRequest<List<SemestreAcompanhamentoDto>>
    {
        public ObterListaSemestresRelatorioPAPQuery(int bimestreAtual)
        {
            if (bimestreAtual <= 0)
                throw new NegocioException("Para obter a lista de bimestres é necessário informar o bimestre atual");

            BimestreAtual = bimestreAtual;
        }

        public int BimestreAtual { get; set; }
    }
}
