using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AulaDeExperienciaPedagogicaQueryHandler : IRequestHandler<AulaDeExperienciaPedagogicaQuery, bool>
    {
        public Task<bool> Handle(AulaDeExperienciaPedagogicaQuery request, CancellationToken cancellationToken)
            => Task.FromResult(new long[] { 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223 }
                .Contains(request.ComponenteCurricular));
    }
}
