using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes
{
    public class MoverArquivosTemporariosCommandHandlerFake : IRequestHandler<MoverArquivosTemporariosCommand, string>
    {
        public async Task<string> Handle(MoverArquivosTemporariosCommand request, CancellationToken cancellationToken)
        {
            return request.TextoEditorNovo;
        }
    }
}
