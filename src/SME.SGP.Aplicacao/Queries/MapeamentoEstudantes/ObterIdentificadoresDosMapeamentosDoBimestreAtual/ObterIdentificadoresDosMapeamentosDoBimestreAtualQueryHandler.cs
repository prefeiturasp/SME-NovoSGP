using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadoresDosMapeamentosDoBimestreAtualQueryHandler : IRequestHandler<ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery, IEnumerable<long>>
    {
        private readonly IRepositorioMapeamentoEstudante repositorio;

        public ObterIdentificadoresDosMapeamentosDoBimestreAtualQueryHandler(IRepositorioMapeamentoEstudante repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<long>> Handle(ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterIdentificadoresDosMapeamentosDoBimestreAtual();
        }
    }
}
