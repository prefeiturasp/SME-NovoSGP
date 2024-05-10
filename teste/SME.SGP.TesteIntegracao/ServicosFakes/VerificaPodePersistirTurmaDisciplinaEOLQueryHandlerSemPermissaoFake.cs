using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerSemPermissaoFake : IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>
    {
        public VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerSemPermissaoFake()
        {}

        public async Task<bool> Handle(VerificaPodePersistirTurmaDisciplinaEOLQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(false);
        }
    }
}
