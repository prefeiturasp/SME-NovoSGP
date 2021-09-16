using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandler
        : IRequestHandler<ObterNotaTipoPorAnoModalidadeDataReferenciaQuery, NotaTipoValor>
    {
        private readonly IRepositorioCiclo _repositorioCiclo;
        private readonly IRepositorioNotaTipoValor _repositorioNotaTipoValor;

        public ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandler(IRepositorioCiclo repositorioCiclo,
            IRepositorioNotaTipoValor repositorioNotaTipoValor)
        {
            _repositorioNotaTipoValor = repositorioNotaTipoValor;
            _repositorioCiclo = repositorioCiclo;
        }
        public Task<NotaTipoValor> Handle(ObterNotaTipoPorAnoModalidadeDataReferenciaQuery request, CancellationToken cancellationToken)
        {
            var anoCicloModalidade = request.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : request.Ano;
            var ciclo = _repositorioCiclo.ObterCicloPorAnoModalidade(anoCicloModalidade, request.Modalidade);
            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");
            return Task.FromResult(_repositorioNotaTipoValor.ObterPorCicloIdDataAvalicacao(ciclo.Id, request.DataReferencia));
        }
    }
}
