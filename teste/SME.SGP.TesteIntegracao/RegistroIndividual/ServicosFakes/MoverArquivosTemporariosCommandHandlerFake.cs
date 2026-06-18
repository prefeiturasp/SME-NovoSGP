using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

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
