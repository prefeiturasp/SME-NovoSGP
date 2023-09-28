using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.ServicosFake
{
    public class ObterDescricaoComponenteCurricularPorIdQueryHandlerCompInfantilFake : IRequestHandler<ObterDescricaoComponenteCurricularPorIdQuery, string>
    {
        public async Task<string> Handle(ObterDescricaoComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id != 1)
                throw new NegocioException("Descrição do componente não localizado.");

            return await Task.FromResult("Comp Infantil");
        }
    }
}
