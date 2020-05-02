using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        public static async Task<IEnumerable<EventoAulaDiaDto>> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId,
            int mes, IServicoUsuario servicoUsuario, IServicoEOL servicoEOL)
        {

            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var aulas = await mediator.Send(new ObterAulasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes
            });

            IEnumerable<Aula> aulasParaVisualizar = await ObterAulasParaVisualizacao(usuarioLogado, aulas, servicoEOL, servicoUsuario, filtroAulasEventosCalendarioDto);

            var avaliacoes = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });

            var qntDiasMes = DateTime.DaysInMonth(filtroAulasEventosCalendarioDto.AnoLetivo, mes);

            var listaRetorno = new List<EventoAulaDiaDto>();
            for (int i = 1; i < qntDiasMes + 1; i++)
            {
                var eventoAula = new EventoAulaDiaDto() { Dia = i };

                if (eventosDaUeSME.Any(a => i >= a.DataInicio.Day && i <= a.DataFim.Day))
                    eventoAula.TemEvento = true;

                var aulasDoDia = aulasParaVisualizar.Where(a => a.DataAula.Day == i);
                if (aulasDoDia.Any())
                {
                    if (aulasDoDia.Any(a => a.AulaCJ))
                        eventoAula.TemAulaCJ = true;

                    if (aulasDoDia.Any(a => a.AulaCJ == false))
                        eventoAula.TemAula = true;

                    if (eventoAula.TemAula || eventoAula.TemAulaCJ)
                    {

                        var avaliacoesDoDia = avaliacoes.Where(a => a.DataAvaliacao.Day == i);
                        var componentesCurricularesDoDia = aulasDoDia.Select(a => a.DisciplinaId);
                        if (avaliacoesDoDia.Any())
                        {
                            var temAvaliacaoComComponente = (from avaliacao in avaliacoesDoDia
                                                             from disciplina in avaliacao.Disciplinas
                                                             where componentesCurricularesDoDia.Contains(disciplina.DisciplinaId.ToString()) || avaliacao.ProfessorRf == usuarioLogado.CodigoRf
                                                             select true);                            

                            if (temAvaliacaoComComponente.Any())
                                eventoAula.TemAvaliacao = true;

                        }

                    }
                }
                listaRetorno.Add(eventoAula);
            }

            return listaRetorno.ToArray();
        }        

        private static async Task<IEnumerable<Aula>> ObterAulasParaVisualizacao(Usuario usuarioLogado, IEnumerable<Aula> aulas, IServicoEOL servicoEOL, 
            IServicoUsuario servicoUsuario, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto)
        {
            IEnumerable<Aula> aulasParaVisualizar;


            if (usuarioLogado.TemPerfilGestaoUes())
            {
                aulasParaVisualizar = aulas.ToList();
            }
            else
            {
                if (usuarioLogado.EhProfessorCj())
                {
                    aulasParaVisualizar = aulas.Where(a => a.ProfessorRf == usuarioLogado.CodigoRf).ToList();
                }
                else
                {
                    aulasParaVisualizar = await ObterAulasParaVisualizarDoProfessor(aulas, usuarioLogado, servicoUsuario, servicoEOL, filtroAulasEventosCalendarioDto);
                }
            }

            return aulasParaVisualizar;
        }

        private static async Task<IEnumerable<Aula>> ObterAulasParaVisualizarDoProfessor(IEnumerable<Aula> aulas, Usuario usuarioLogado, IServicoUsuario servicoUsuario, IServicoEOL servicoEOL, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto )
        {
            
            string[] componentesCurricularesParaVisualizar = new string[0];

            if (usuarioLogado.EhProfessor())
                componentesCurricularesParaVisualizar = await servicoUsuario.ObterComponentesCurricularesQuePodeVisualizarHoje(filtroAulasEventosCalendarioDto.TurmaCodigo, usuarioLogado);

            var componentesCurricularesDasAulas = aulas.Select(a => a.DisciplinaId);
            
            return aulas.Where(a => componentesCurricularesParaVisualizar.Contains(a.DisciplinaId) || a.ProfessorRf == usuarioLogado.CodigoRf);            
        }
        
    }
}
