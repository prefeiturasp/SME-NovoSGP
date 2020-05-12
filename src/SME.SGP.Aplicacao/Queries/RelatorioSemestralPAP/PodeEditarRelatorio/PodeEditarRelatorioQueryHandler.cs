using MediatR;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodeEditarRelatorioQueryHandler : IRequestHandler<PodeEditarRelatorioQuery, bool>
    {
        public Task<bool> Handle(PodeEditarRelatorioQuery request, CancellationToken cancellationToken)
        {
            if (request.Turma.ModalidadeCodigo != Modalidade.EJA)
            {
                return Task.FromResult(request.BimestreAtual == 2 || request.BimestreAtual == 4);
            }
            return Task.FromResult(true);
            ;
        }
    }
}
