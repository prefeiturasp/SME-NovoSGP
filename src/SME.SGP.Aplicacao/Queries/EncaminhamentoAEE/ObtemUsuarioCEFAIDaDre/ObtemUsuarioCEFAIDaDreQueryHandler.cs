using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObtemUsuarioCEFAIDaDreQueryHandler : IRequestHandler<ObtemUsuarioCEFAIDaDreQuery, long>
    {
        private readonly IMediator mediator;

        public ObtemUsuarioCEFAIDaDreQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(ObtemUsuarioCEFAIDaDreQuery request, CancellationToken cancellationToken)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(request.CodigoDRE, new List<Guid>() { Perfis.PERFIL_CEFAI }));

            if (!funcionarios.Any())
                return 0;

            var funcionario = funcionarios.FirstOrDefault();
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionario));

            return usuarioId;
        }
    }
}
