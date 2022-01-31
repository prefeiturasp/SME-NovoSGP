using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommand : IRequest<AuditoriaDto>
    {
        public InserirFrequenciasAulaCommand(FrequenciaDto frequencia)
        {
            Frequencia = frequencia;
        }

        public FrequenciaDto Frequencia { get; }
    }
}
