using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao
{
    public class ObterPerfilAtualProfessorQueryHandlerFake : IRequestHandler<ObterPerfilAtualQuery, Guid>
    {
        public async Task<Guid> Handle(ObterPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Guid.Parse(PerfilUsuario.PROFESSOR.Name()));
        }
    }
}