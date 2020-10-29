using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosComComponentePorPlanoAulaIdQueryHandler : IRequestHandler<ObterObjetivosComComponentePorPlanoAulaIdQuery, IEnumerable<ObjetivoAprendizagemComponenteDto>>
    {
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivoAprendizagemAula;

        public ObterObjetivosComComponentePorPlanoAulaIdQueryHandler(IRepositorioObjetivoAprendizagemAula repositorioObjetivoAprendizagemAula)
        {
            this.repositorioObjetivoAprendizagemAula = repositorioObjetivoAprendizagemAula ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagemAula));
        }

        public Task<IEnumerable<ObjetivoAprendizagemComponenteDto>> Handle(ObterObjetivosComComponentePorPlanoAulaIdQuery request, CancellationToken cancellationToken)
            => repositorioObjetivoAprendizagemAula.ObterObjetivosComComponentePlanoAula(request.PlanoAulaId);
    }
}
