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
            if (turma.EhNulo())
                throw new NegocioException("A turma informada não foi encontrada!");

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var turmaPossuiComponente = await mediator.Send(new TurmaPossuiComponenteCurricularPAPQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            if (!turmaPossuiComponente)
                return null;

            var bimestreAtual = await mediator.Send(new ObterBimestreAtualQuery(DateTime.Today, turma));

            var semestres = await mediator.Send(new ObterListaSemestresRelatorioPAPQuery(bimestreAtual));
            var tipoCalendarioTurmaId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            bool temPermissao = VerificaSeBimestreAtualPossuiPermissaoEdicao(semestres, bimestreAtual);

            if (tipoCalendarioTurmaId > 0 && (!temPermissao || semestres.Any(x => x.PodeEditar == false)))
            {
                var semestresComReaberturaVigente = await mediator.Send(new ObterSemestresComReaberturaAtivaPAPQuery(DateTime.Now, tipoCalendarioTurmaId, turma.UeId, semestres));

                if (semestresComReaberturaVigente.Any())
                    return semestresComReaberturaVigente.ToList();
            }

            return semestres;
        }

        private static bool VerificaSeBimestreAtualPossuiPermissaoEdicao(List<SemestreAcompanhamentoDto> semestres, int bimestreAtual)
        {
            if (semestres.Any())
            {
                if (bimestreAtual == 1 || bimestreAtual == 2)
                    return semestres.FirstOrDefault(s => s.Semestre == 1).PodeEditar;
                else if (bimestreAtual == 3 || bimestreAtual == 4)
                    return semestres.FirstOrDefault(s => s.Semestre == 2).PodeEditar;
            } 

            return false;           
        }
    }
            
}
