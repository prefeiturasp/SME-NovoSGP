using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmaComUsuarioQueryHandler : IRequestHandler<ObterAbrangenciaTurmaComUsuarioQuery, AbrangenciaFiltroRetorno>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IMediator mediator;

        public ObterAbrangenciaTurmaComUsuarioQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AbrangenciaFiltroRetorno> Handle(ObterAbrangenciaTurmaComUsuarioQuery request,
            CancellationToken cancellationToken)
        {
            var login = request.Login;
            var perfil = request.Perfil;
            AbrangenciaCompactaVigenteRetornoEOLDTO abrangencia = await mediator.Send(new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(login, perfil));
            bool abrangenciaPermitida = abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.UE
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.Dre
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.SME;

            return await repositorioAbrangencia.ObterAbrangenciaTurma(request.TurmaCodigo, login, perfil, request.ConsideraHistorico, abrangenciaPermitida);
        }
    }
}
