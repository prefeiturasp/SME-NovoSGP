using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ProcessoEstaEmExecucaoQuery : IRequest<bool>
    {
        public ProcessoEstaEmExecucaoQuery(TipoProcesso tipoProcesso)
        {
            TipoProcesso = tipoProcesso;
        }

        public TipoProcesso TipoProcesso { get; set; }
    }
}
