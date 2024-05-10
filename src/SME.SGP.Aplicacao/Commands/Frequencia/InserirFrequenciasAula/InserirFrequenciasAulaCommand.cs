using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommand : IRequest<FrequenciaAuditoriaAulaDto>
    {
        public InserirFrequenciasAulaCommand(FrequenciaDto frequencia, bool calcularFrequencia = true)
        {
            Frequencia = frequencia;
            CalcularFrequencia = calcularFrequencia;
        }

        public bool CalcularFrequencia { get; set; }

        public FrequenciaDto Frequencia { get; }
    }
}
