using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake
{
    public class PublicarFilaGoogleClassroomCommandHandlerFake : IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>
    {
        public PublicarFilaGoogleClassroomCommandHandlerFake()
        {
        }

        public async Task<bool> Handle(PublicarFilaGoogleClassroomCommand request, CancellationToken cancellationToken)
        {
            var retorno = true;
            return retorno;
        }
    }
}
