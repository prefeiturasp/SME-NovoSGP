using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaUseCase : IObterTurmasParaCopiaUseCase
    {
        private readonly IMediator mediator;

        public ObterTurmasParaCopiaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Executar(int turmaId, long componenteCurricularId, bool ensinoEspecial, bool consideraHistorico)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario.PerfilAtual == Dominio.Perfis.PERFIL_CP)
                return await mediator.Send(new ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery(turmaId, ensinoEspecial, consideraHistorico));


            return await mediator.Send(new ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery(turmaId, componenteCurricularId, usuario.CodigoRf, consideraHistorico));
        }
    }
}
