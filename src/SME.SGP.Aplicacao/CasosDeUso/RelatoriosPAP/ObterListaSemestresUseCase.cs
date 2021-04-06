using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresUseCase
    {
        public static async Task<List<SemestreAcompanhamentoDto>> Executar(IMediator mediator, string turmaCodigo)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var turmaPossuiComponente = await mediator.Send(new TurmaPossuiComponenteCurricularPAPQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            if (!turmaPossuiComponente)
                return null;

            var bimestreAtual = await mediator.Send(new ObterBimestreAtualQuery(turmaCodigo, DateTime.Today));

            return await mediator.Send(new ObterListaSemestresRelatorioPAPQuery(bimestreAtual));
        }
    }
}
