using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVersaoPlanoAEEPorIdQueryHandler : IRequestHandler<ObterVersaoPlanoAEEPorIdQuery, PlanoAEEVersaoDto>
    {
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public ObterVersaoPlanoAEEPorIdQueryHandler(IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
        }

        public async Task<PlanoAEEVersaoDto> Handle(ObterVersaoPlanoAEEPorIdQuery request, CancellationToken cancellationToken)
        {
            var versaoPlano = await repositorioPlanoAEEVersao.ObterPorIdAsync(request.VersaoPlanoId);

            return new PlanoAEEVersaoDto()
            {
                Id = versaoPlano.Id,
                Numero = versaoPlano.Numero,
                AlteradoEm = versaoPlano.AlteradoEm,
                AlteradoPor = versaoPlano.AlteradoPor,
                AlteradoRF = versaoPlano.AlteradoRF,
                CriadoEm = versaoPlano.CriadoEm,
                CriadoPor = versaoPlano.CriadoPor,
                CriadoRF = versaoPlano.CriadoRF,
                PlanoAEEId = versaoPlano.PlanoAEEId
            };
        }
    }
}
