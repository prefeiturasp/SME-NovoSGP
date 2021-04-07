using MediatR;

namespace SME.SGP.Aplicacao
{
    public class EnviarSincronizacaoEstruturaInstitucionalUesCommand : IRequest<bool>
    {
        public EnviarSincronizacaoEstruturaInstitucionalUesCommand(string codigoDre)
        {
            CodigoDre = codigoDre;
        }

        public string CodigoDre { get; set; }
    }
}
