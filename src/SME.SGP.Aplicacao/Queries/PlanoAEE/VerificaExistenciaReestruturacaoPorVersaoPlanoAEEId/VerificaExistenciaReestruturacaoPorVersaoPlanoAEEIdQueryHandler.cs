using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQueryHandler : IRequestHandler<VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery, bool>
    {
        private readonly IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao;

        public VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQueryHandler(IRepositorioPlanoAEEReestruturacao repositorioPlanoAEEReestruturacao)
        {
            this.repositorioPlanoAEEReestruturacao = repositorioPlanoAEEReestruturacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEReestruturacao));
        }

        public async Task<bool> Handle(VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEReestruturacao.ExisteReestruturacaoParaVersao(request.VersaoId, request.ReestruturacaoId);
    }
}
