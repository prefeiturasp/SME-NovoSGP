using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class MoverArquivosTemporariosCommandHandlerFake : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        public async Task<string> Handle(MoverArquivosTemporariosCommand request, CancellationToken cancellationToken)
        {
            return "Lorem Ipsum";
        }
    }
}