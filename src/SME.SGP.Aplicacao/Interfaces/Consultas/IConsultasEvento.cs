using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEvento
    {
        EventoObterParaEdicaoDto ObterPorId(long id);
    }
}