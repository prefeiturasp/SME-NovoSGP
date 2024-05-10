using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery : IRequest<IEnumerable<TipoCalendarioRetornoDto>>
    {
        public ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery(int anoLetivo, IEnumerable<int> modalidades, string descricao)
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
            Descricao = descricao;
        }

        public int AnoLetivo { get; set; }
        public IEnumerable<int> Modalidades { get; set; }
        public string Descricao { get; set; }
    }
}
