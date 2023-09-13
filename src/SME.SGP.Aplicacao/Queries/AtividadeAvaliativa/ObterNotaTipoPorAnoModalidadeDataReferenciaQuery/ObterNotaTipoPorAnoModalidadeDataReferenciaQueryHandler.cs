using System;
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
        private readonly IRepositorioNotaTipoValorConsulta _repositorioNotaTipoValor;
        private readonly IMediator mediator;

        public ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandler(
            IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor,IMediator mediator)
        {
            _repositorioNotaTipoValor = repositorioNotaTipoValor;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<NotaTipoValor> Handle(ObterNotaTipoPorAnoModalidadeDataReferenciaQuery request, CancellationToken cancellationToken)
        {
            var anoCicloModalidade = request.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : request.Ano;
            var ciclo = await mediator.Send(new ObterCicloPorAnoModalidadeQuery(anoCicloModalidade, request.Modalidade));
            
            if (ciclo.EhNulo())
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");
            
            var retorno = await mediator.Send(new ObterNotaTipoPorCicloIdDataAvalicacaoQuery(ciclo.Id, request.DataReferencia));
            return retorno;
        }
    }
}
