using SME.SGP.Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoBimestre : IRepositorioBase<EventoBimestre>
    {
        Task ExcluiEventoBimestre(long eventoId);
        Task<int[]> ObterEventoBimestres(long eventoId);
        Task<int[]> ObterBimestresEventoPorDataRefencia(DateTime dataReferencia);
    }
}
