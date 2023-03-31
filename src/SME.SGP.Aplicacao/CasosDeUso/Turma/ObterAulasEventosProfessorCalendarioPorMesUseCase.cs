using MediatR;
using SME.SGP.Dominio;
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

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
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

            var componentesCurricularesDoProfessor = new List<(string codigo, string codigoTerritorioSaber)>();

            bool verificaCJPodeEditar = await VerificaCJPodeEditarRegistroTitular(filtroAulasEventosCalendarioDto.AnoLetivo);

            IEnumerable<Aula> aulasParaVisualizar;
            IEnumerable<ComponenteCurricularEol> componentesCurricularesEolProfessor = new List<ComponenteCurricularEol>();

            if (usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
                aulasParaVisualizar = aulas;
            else
            {
                componentesCurricularesEolProfessor = (await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(filtroAulasEventosCalendarioDto.TurmaCodigo,
                                                                                 usuarioLogado.CodigoRf,
                                                                                 usuarioLogado.PerfilAtual,
                                                                                 usuarioLogado.EhProfessorInfantilOuCjInfantil()))).ToList();

                if (usuarioLogado.EhSomenteProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    if (componentesCurricularesDoProfessorCJ.Any())
                    {
                        var dadosComponentes = await mediator
                            .Send(new ObterDisciplinasPorIdsQuery(componentesCurricularesDoProfessorCJ.Select(c => c.DisciplinaId).ToArray()));

                        if (dadosComponentes.Any())
                        {
                            componentesCurricularesDoProfessor.AddRange(dadosComponentes
                                .Select(d => (d.CodigoComponenteCurricular.ToString(), d.TerritorioSaber ? d.CodigoComponenteCurricular.ToString() : "0")));
                        }
                    }
                }

                if (componentesCurricularesEolProfessor != null && componentesCurricularesEolProfessor.Any())
                {
                    componentesCurricularesEolProfessor.ToList()
                        .ForEach(cc => componentesCurricularesDoProfessor
                            .Add((cc.Codigo.ToString(), cc.CodigoComponenteTerritorioSaber.ToString())));
                }

                aulasParaVisualizar = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, componentesCurricularesDoProfessor);

                avaliacoes = usuarioLogado
                    .ObterAtividadesAvaliativasQuePodeVisualizar(avaliacoes, componentesCurricularesDoProfessor
                        .Select(c => c.codigo)
                            .ToArray());
            }

            return await mediator.Send(new ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Aulas = aulasParaVisualizar,
                EventosDaUeSME = eventosDaUeSME,
                Avaliacoes = avaliacoes,
                UsuarioCodigoRf = usuarioLogado.CodigoRf,
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