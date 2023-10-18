using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesUseCase : AbstractUseCase, IObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        public ObterAulasEventosProfessorCalendarioPorMesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<EventoAulaDiaDto>> Executar(FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes)
        {
            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhNulo())
                throw new NegocioException("Não foi possível localizar o Usuário logado.");

            var aulas = await mediator.Send(new ObterAulasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes
            });

            aulas = aulas.Where(a => a.TipoCalendarioId == tipoCalendarioId).ToList();
            var avaliacoes = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });            

            bool verificaCJPodeEditar = await VerificaCJPodeEditarRegistroTitular(filtroAulasEventosCalendarioDto.AnoLetivo);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtroAulasEventosCalendarioDto.TurmaCodigo));
            IEnumerable<Aula> aulasParaVisualizar;
            var componentesCurricularesEolProfessor = new List<ComponenteCurricularEol>();

            if (usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
                aulasParaVisualizar = aulas;
            else
            {
                componentesCurricularesEolProfessor = (await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma,
                                                                                 usuarioLogado.Login,
                                                                                 usuarioLogado.PerfilAtual,
                                                                                 turma.EhTurmaInfantil))).ToList();

                var componentesCurricularesAgrupamentoTerritorioSaber = componentesCurricularesEolProfessor.Where(cc => cc.Codigo.EhIdComponenteCurricularTerritorioSaberAgrupado());
                if (componentesCurricularesAgrupamentoTerritorioSaber.Any())
                    componentesCurricularesEolProfessor.AddRange(await mediator.Send(new ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(componentesCurricularesAgrupamentoTerritorioSaber.Select(cc => cc.Codigo).ToArray())));


                if (usuarioLogado.EhSomenteProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    if (componentesCurricularesDoProfessorCJ.Any())
                    {
                        componentesCurricularesDoProfessorCJ.ToList().ForEach(ccj =>
                        {
                            componentesCurricularesEolProfessor.Add(new ComponenteCurricularEol()
                            {
                                Codigo = ccj.DisciplinaId,
                                Professor = ccj.ProfessorRf
                            });
                        });
                    }
                }

                aulasParaVisualizar = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, componentesCurricularesEolProfessor);

                // códigos disciplinas normais + regência + território
                var codigosComponentesUsuario = componentesCurricularesEolProfessor.Select(c => c.Codigo.ToString())
                    .Concat(componentesCurricularesEolProfessor.Where(c => c.Regencia && c.CodigoComponenteCurricularPai.HasValue && c.CodigoComponenteCurricularPai.Value > 0).Select(c => c.CodigoComponenteCurricularPai.Value.ToString()))
                    .Concat(componentesCurricularesEolProfessor.Where(c => c.TerritorioSaber).Select(c => c.CodigoComponenteTerritorioSaber.ToString()))                    
                    .ToArray();

                avaliacoes = usuarioLogado
                    .ObterAtividadesAvaliativasQuePodeVisualizar(avaliacoes, codigosComponentesUsuario);
            }

            return await mediator.Send(new ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Aulas = aulasParaVisualizar,
                EventosDaUeSME = eventosDaUeSME,
                Avaliacoes = avaliacoes,
                UsuarioCodigoRf = usuarioLogado.CodigoRf ?? usuarioLogado.Login,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });
        }

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
        }
    }
}