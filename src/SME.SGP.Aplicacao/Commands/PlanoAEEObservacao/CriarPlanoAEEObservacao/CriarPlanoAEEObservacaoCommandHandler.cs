using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarPlanoAEEObservacaoCommandHandler : IRequestHandler<CriarPlanoAEEObservacaoCommand, AuditoriaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;

        public CriarPlanoAEEObservacaoCommandHandler(IUnitOfWork unitOfWork, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
        }

        public async Task<AuditoriaDto> Handle(CriarPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            var observacao = new PlanoAEEObservacao(request.PlanoAEEId, request.Observacao);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPlanoAEEObservacao.SalvarAsync(observacao);
                    // Gerar notificação 

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }                
            }

            return (AuditoriaDto)observacao;
        }
    }
}
