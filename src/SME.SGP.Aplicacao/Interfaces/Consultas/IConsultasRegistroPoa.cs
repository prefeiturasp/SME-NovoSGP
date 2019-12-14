using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasRegistroPoa
    {
        RegistroPoaCompletoDto ObterPorId(long id);
    }
}