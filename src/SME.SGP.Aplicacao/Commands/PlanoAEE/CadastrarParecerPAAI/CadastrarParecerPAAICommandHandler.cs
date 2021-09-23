using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarParecerPAAICommandHandler : IRequestHandler<CadastrarParecerPAAICommand, bool>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;

        public CadastrarParecerPAAICommandHandler(
            IMediator mediator,
            IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Handle(CadastrarParecerPAAICommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(request.PlanoAEEId);

            if (planoAEE == null)
                throw new NegocioException("Plano AEE não encontrado!");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            if (planoAEE.ResponsavelId != usuarioLogado)
                throw new NegocioException("O usuário atual não é o PAAI responsável por este Plano AEE");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.Validado;
            planoAEE.ParecerPAAI = request.ParecerPAAI;

            var idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await ExcluirPendenciaPAAI(planoAEE);            

            return idEntidadeEncaminhamento != 0;
        }      

        private async Task ExcluirPendenciaPAAI(PlanoAEE planoAEE)
            => await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAEE.Id));
    }
}
