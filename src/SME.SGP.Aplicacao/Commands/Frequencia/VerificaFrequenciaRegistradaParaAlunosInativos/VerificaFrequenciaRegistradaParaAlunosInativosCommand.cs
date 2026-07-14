using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaFrequenciaRegistradaParaAlunosInativosCommand : IRequest<bool>
    {
        public string TurmaCodigo { get; set; }
    }
}
