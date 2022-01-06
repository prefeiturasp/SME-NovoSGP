using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        public static async Task<IEnumerable<EventoAulaDiaDto>> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId,
            int mes, IServicoUsuario servicoUsuario)
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
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes
            });

            var avaliacoes = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });

            IEnumerable<Aula> aulasParaVisualizar = null;

            string[] componentesCurricularesDoProfessor = new string[0];
            
            if (usuarioLogado.EhProfessor())
            {
                componentesCurricularesDoProfessor = await mediator.Send(new ObterComponentesCurricularesQuePodeVisualizarHojeQuery(usuarioLogado.CodigoRf,usuarioLogado.PerfilAtual, filtroAulasEventosCalendarioDto.TurmaCodigo));
                aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulas, componentesCurricularesDoProfessor);
                avaliacoes = usuarioLogado.ObterAtividadesAvaliativasQuePodeVisualizar(avaliacoes, componentesCurricularesDoProfessor);
            }                

            if (usuarioLogado.EhProfessorCjInfantil())
            {
                //verificar a questão do ano, para barrar esse tratamento para anos anteriores... (vigência)
                var professoresTitularesComponentesCJ = new List<ProfessorTitularDisciplinaEol>();

                var componentesAtribuidosCJ = await mediator.Send(new ObterAtribuicaoCJPorDreUeTurmaRFQuery(filtroAulasEventosCalendarioDto.TurmaCodigo, 
                    filtroAulasEventosCalendarioDto.DreCodigo, filtroAulasEventosCalendarioDto.UeCodigo, usuarioLogado.CodigoRf));

                foreach (var dados in componentesAtribuidosCJ)
                    professoresTitularesComponentesCJ.Add(await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(filtroAulasEventosCalendarioDto.TurmaCodigo, 
                        dados.DisciplinaId.ToString())));

                
                aulasParaVisualizar = from aula in aulas 
                                      join profTitular in professoresTitularesComponentesCJ 
                                      on new { DisciplinaId = long.Parse(aula.DisciplinaId), aula.ProfessorRf } equals new { profTitular.DisciplinaId, profTitular.ProfessorRf }
                                      select aula;

                //var disciplinasDasAvaliacoes = from avaliacao in avaliacoes
                //             join profTitular in professoresTitularesComponentesCJ
                //             on new { avaliacao.ProfessorRf } equals new { profTitular.ProfessorRf }
                //             select avaliacao.Disciplinas.;

                //avaliacoes = from disciplina in disciplinasDasAvaliacoes
                //             join profTitular in professoresTitularesComponentesCJ
                //             on new { disciplina. } equals new { profTitular.ProfessorRf }
                //             select disciplina.Disciplinas;

                //avaliacoes.Where(a => (componentesCurricularesProfessor.Intersect(a.Disciplinas.Select(d => d.DisciplinaId)).Any() && !a.EhCj || a.Disciplinas.Select(item => item.DisciplinaId).Any() && a.EhCj) || a.ProfessorRf == CodigoRf);
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
    }
}