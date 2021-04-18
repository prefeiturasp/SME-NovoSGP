using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarPlanoAEEObservacaoCommandHandler : IRequestHandler<AlterarPlanoAEEObservacaoCommand, AuditoriaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;

        public AlterarPlanoAEEObservacaoCommandHandler(IUnitOfWork unitOfWork, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
        }

        public async Task<AuditoriaDto> Handle(AlterarPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            var observacaoPlano = await repositorioPlanoAEEObservacao.ObterPorIdAsync(request.Id);
            observacaoPlano.Observacao = request.Observacao;

            using(var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPlanoAEEObservacao.SalvarAsync(observacaoPlano);
                    // Notificar Usuários
                    
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return (AuditoriaDto)observacaoPlano;
        }
    }
}
