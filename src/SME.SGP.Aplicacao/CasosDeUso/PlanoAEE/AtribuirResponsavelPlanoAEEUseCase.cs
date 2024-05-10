using System;
using MediatR;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEEUseCase : AbstractUseCase, IAtribuirResponsavelPlanoAEEUseCase
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        public AtribuirResponsavelPlanoAEEUseCase(IMediator mediator,IRepositorioPlanoAEE repositorioPlanoAEE) : base(mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Executar(long planoAEEId, string responsavelRF)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(planoAEEId);

            if (planoAEE.EhNulo())
                throw new NegocioException(MensagemNegocioPlanoAee.Plano_aee_nao_encontrado);
            
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);
            
            return await mediator.Send(new AtribuirResponsavelPlanoAEECommand(planoAEE, responsavelRF, turma));
        }
    }
}
