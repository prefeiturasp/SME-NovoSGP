using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAulasPorTurmaEComponentePeriodoUseCase : AbstractUseCase, IObterPlanoAulasPorTurmaEComponentePeriodoUseCase
    {
        private readonly IServicoUsuario servicoUsuario;

        public ObterPlanoAulasPorTurmaEComponentePeriodoUseCase(IMediator mediator, IServicoUsuario servicoUsuario) : base(mediator) 
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<PlanoAulaRetornoDto>> Executar(FiltroObterPlanoAulaPeriodoDto param)
        {
            return await mediator.Send(new ObterPlanoAulasPorTurmaEComponentePeriodoQuery(param.TurmaCodigo, 
                                                                                          param.ComponenteCurricularCodigo,
                                                                                          param.ComponenteCurricularId,
                                                                                          param.AulaInicio,
                                                                                          param.AulaFim));
        } 
    }
}
