using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdAnoLetivoModalidadeTipoQuery : IRequest<Turma>
    {
        public ObterTurmaPorIdAnoLetivoModalidadeTipoQuery(long ueId, int anoLetivo, TipoTurma turmaTipo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            TurmaTipo = turmaTipo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }       
        public TipoTurma TurmaTipo { get; set; }
    }
}
