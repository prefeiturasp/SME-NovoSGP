using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEvento
    {
        EventoDto ObterPorId(long id);
    }
}