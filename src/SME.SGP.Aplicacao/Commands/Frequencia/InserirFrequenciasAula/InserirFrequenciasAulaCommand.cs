using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommand : IRequest<FrequenciaAuditoriaAulaDto>
    {
        public InserirFrequenciasAulaCommand(FrequenciaDto frequencia, bool calcularFrequencia = true, string usuarioLogin = "Sistema")
        {
            Frequencia = frequencia;
            CalcularFrequencia = calcularFrequencia;
            UsuarioLogin = usuarioLogin;
        }

        public bool CalcularFrequencia { get; set; }

        public FrequenciaDto Frequencia { get; }

        public string UsuarioLogin { get; set; }
    }
}
