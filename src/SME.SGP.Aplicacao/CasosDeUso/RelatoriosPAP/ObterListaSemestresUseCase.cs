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
            var tipoCalendarioTurma = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            if(tipoCalendarioTurma > 0)
            {
                var periodoFechamentoReaberturaVigente = await mediator.Send(new ObterFechamentoReaberturaPorDataTurmaQuery() { DataParaVerificar = DateTime.Now.Date, TipoCalendarioId = tipoCalendarioTurma, UeId = turma.UeId });

                if (periodoFechamentoReaberturaVigente != null)
                {
                    var bimestresComReabertura = await mediator.Send(new ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery(periodoFechamentoReaberturaVigente.Id));

                    if (bimestresComReabertura.Any())
                    {
                        var semestresAjustadosComReabertura = semestres.Select(s => new SemestreAcompanhamentoDto()
                        {
                            Descricao = s.Descricao,
                            Semestre = s.Semestre,
                            PodeEditar = s.Semestre == 1 && bimestresComReabertura.Any(b => b.Bimestre == 2) ? true
                       : s.Semestre == 2 && bimestresComReabertura.Any(b => b.Bimestre == 4) ? true : false
                        });

                        return semestresAjustadosComReabertura.ToList();
                    }           
                }
            }

            return semestres;
        }
    }
            
}
