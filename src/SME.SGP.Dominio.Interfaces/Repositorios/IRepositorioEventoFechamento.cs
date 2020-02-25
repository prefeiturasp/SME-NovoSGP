using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoFechamento : IRepositorioBase<EventoFechamento>
    {
        EventoFechamento ObterPorIdFechamento(long fechamentoId);
        Task<bool> UeEmFechamento(DateTime dataReferencia, string dreCodigo, string ueCodigo, int bimestre, long tipoCalendarioId);
    }
}