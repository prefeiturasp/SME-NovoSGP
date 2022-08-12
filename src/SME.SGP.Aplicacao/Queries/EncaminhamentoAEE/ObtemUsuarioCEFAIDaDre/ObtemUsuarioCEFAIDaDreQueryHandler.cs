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
    public class ObtemUsuarioCEFAIDaDreQueryHandler : IRequestHandler<ObtemUsuarioCEFAIDaDreQuery, IEnumerable<long>>
    {
        private readonly IMediator mediator;

        public ObtemUsuarioCEFAIDaDreQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<long>> Handle(ObtemUsuarioCEFAIDaDreQuery request, CancellationToken cancellationToken)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(request.CodigoDRE, new List<Guid>() { Perfis.PERFIL_CEFAI }));

            return await ObterUsuarios(funcionarios);
        }

        private async Task<IEnumerable<long>> ObterUsuarios(IEnumerable<string> funcionarios)
        {
            var usuarios = new List<long>();

            foreach(var funcionario in funcionarios)
                usuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionario)));

            return usuarios;
        }
    }
}
