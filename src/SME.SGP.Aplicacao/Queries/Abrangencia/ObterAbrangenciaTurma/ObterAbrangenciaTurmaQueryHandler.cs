using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmaQueryHandler : IRequestHandler<ObterAbrangenciaTurmaQuery, AbrangenciaFiltroRetorno>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaTurmaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<AbrangenciaFiltroRetorno> Handle(ObterAbrangenciaTurmaQuery request, CancellationToken cancellationToken)
        {
            var abrangenciaTurma = await repositorioAbrangencia.ObterAbrangenciaTurma(request.TurmaCodigo,
                                                                                      request.Login,
                                                                                      request.Perfil,
                                                                                      request.ConsideraHistorico,
                                                                                      request.AbrangenciaPermitida);
            return abrangenciaTurma;
        }
    }
}
