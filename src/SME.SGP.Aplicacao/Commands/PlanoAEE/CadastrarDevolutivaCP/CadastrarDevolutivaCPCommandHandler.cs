using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarDevolutivaCPCommandHandler : IRequestHandler<CadastrarDevolutivaCPCommand, bool>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public CadastrarDevolutivaCPCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

       
        public async Task<bool> Handle(CadastrarDevolutivaCPCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(request.PlanoAEEId);

            if (planoAEE == null)
                throw new NegocioException("Plano AEE não encontrado!");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.AtribuicaoPAAI;
            planoAEE.ParecerCoordenacao = request.ParecerCoordenacao;

            var idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            return idEntidadeEncaminhamento != 0;
        }
    }
}
