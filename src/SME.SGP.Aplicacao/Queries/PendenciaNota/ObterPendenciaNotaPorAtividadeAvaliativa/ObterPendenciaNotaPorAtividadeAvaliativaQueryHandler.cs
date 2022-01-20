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
    public class ObterPendenciaNotaPorAtividadeAvaliativaQueryHandler : IRequestHandler<ObterPendenciaNotaPorAtividadeAvaliativaQuery, bool>
    {
        private IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterPendenciaNotaPorAtividadeAvaliativaQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<bool> Handle(ObterPendenciaNotaPorAtividadeAvaliativaQuery request, CancellationToken cancellationToken)
            => await repositorioAtividadeAvaliativa.VerificaAtividadesAvaliativasSemNotaParaNenhumAlunoNoBimestre(request.TurmaCodigo, request.DisciplinaId, request.InicioPeriodo, request.FimPeriodo, request.TipoAtividadeAvaliativa);
    }
}
