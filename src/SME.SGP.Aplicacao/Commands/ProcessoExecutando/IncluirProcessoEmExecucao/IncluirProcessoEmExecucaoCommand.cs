using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class IncluirProcessoEmExecucaoCommand : IRequest<long>
    {
        public IncluirProcessoEmExecucaoCommand(TipoProcesso tipoProcesso)
        {
            TipoProcesso = tipoProcesso;
        }

        public TipoProcesso TipoProcesso { get; set; }
    }
}
