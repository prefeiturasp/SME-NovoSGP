using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresUseCase
    {
        public static async Task<List<SemestreAcompanhamentoDto>> Executar(IMediator mediator, string turmaCodigo)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada!");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var turmaPossuiComponente = await mediator.Send(new TurmaPossuiComponenteCurricularPAPQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            if (!turmaPossuiComponente)
                return null;

            var bimestreAtual = await mediator.Send(new ObterBimestreAtualQuery(DateTime.Today, turma));

            var semestres = await mediator.Send(new ObterListaSemestresRelatorioPAPQuery(bimestreAtual));
            var tipoCalendarioTurmaId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            if(tipoCalendarioTurmaId > 0)
            {
                var semestresComReaberturaVigente = await mediator.Send(new ObterSemestresComReaberturaAtivaPAPQuery(DateTime.Now, tipoCalendarioTurmaId, turma.UeId, semestres));

                if (semestresComReaberturaVigente.Any())
                    return semestresComReaberturaVigente.ToList();
            }

            return semestres;
        }
    }
            
}
