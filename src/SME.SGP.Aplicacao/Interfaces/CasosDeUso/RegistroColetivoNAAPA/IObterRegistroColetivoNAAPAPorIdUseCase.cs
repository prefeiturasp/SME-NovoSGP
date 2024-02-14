using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IObterRegistroColetivoNAAPAPorIdUseCase : IUseCase<long, RegistroColetivoCompletoDto>
    {
    }
}