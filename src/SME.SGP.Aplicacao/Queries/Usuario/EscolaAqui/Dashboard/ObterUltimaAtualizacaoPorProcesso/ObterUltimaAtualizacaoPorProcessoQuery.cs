using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaAtualizacaoPorProcessoQuery : IRequest<UltimaAtualizaoWorkerPorProcessoResultado>
    {
        public string NomeProcesso { get; set; }

        public ObterUltimaAtualizacaoPorProcessoQuery(string nomeProcesso)
        {
            NomeProcesso = nomeProcesso;
        }
    }
}
