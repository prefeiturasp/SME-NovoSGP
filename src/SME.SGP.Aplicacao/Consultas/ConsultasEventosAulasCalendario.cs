using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEventosAulasCalendario : IConsultasEventosAulasCalendario
    {
        private readonly IComandosDiasLetivos comandosDiasLetivos;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasEventosAulasCalendario(
            IRepositorioEvento repositorioEvento,
            IComandosDiasLetivos comandosDiasLetivos,
            IRepositorioAula repositorioAula,
            IServicoUsuario servicoUsuario,
            IServicoEOL servicoEOL,
            IConsultasAbrangencia consultasAbrangencia,
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IConsultasDisciplina consultasDisciplina)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.comandosDiasLetivos = comandosDiasLetivos ?? throw new ArgumentNullException(nameof(comandosDiasLetivos));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentException(nameof(repositorioPeriodoEscolar));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<DiaEventoAula> ObterEventoAulasDia(FiltroEventosAulasCalendarioDiaDto filtro)
        {
            List<EventosAulasTipoDiaDto> eventosAulas = new List<EventosAulasTipoDiaDto>();

            if (!filtro.TodasTurmas && string.IsNullOrWhiteSpace(filtro.TurmaId))
                throw new NegocioException("É necessario informar uma turma para pesquisa");

            var temTurmaInformada = !string.IsNullOrEmpty(filtro.TurmaId);
            var data = filtro.Data.Date;

            var perfil = servicoUsuario.ObterPerfilAtual();

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            string rf = usuario.TemPerfilGestaoUes() ? string.Empty : usuario.CodigoRf;

            var disciplinasUsuario = usuario.EhProfessorCj() ?
                await consultasDisciplina.ObterDisciplinasPerfilCJ(filtro.TurmaId, usuario.CodigoRf) :
                await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(filtro.TurmaId, usuario.CodigoRf, usuario.PerfilAtual);

            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeDia(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId, data, filtro.EhEventoSme, usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());
            var aulas = await ObterAulasDia(filtro, data, perfil, rf, disciplinasUsuario);
            var atividades = await repositorioAtividadeAvaliativa.ObterAtividadesPorDia(filtro.DreId, filtro.UeId, data, rf, filtro.TurmaId);

            ObterEventosParaEventosAulasDia(eventosAulas, eventos);

            var turmasAulas = aulas.GroupBy(x => x.TurmaId).Select(x => x.Key);

            var turmasAbrangencia = await ObterTurmasAbrangencia(turmasAulas, filtro.TurmaHistorico);

            var idsDisciplinasAulas = aulas.Select(a => long.Parse(a.DisciplinaId)).Distinct().ToList();

            var idsDisciplinasCompartilhadas = aulas.Where(a => !String.IsNullOrEmpty(a.DisciplinaCompartilhadaId) && !a.DisciplinaCompartilhadaId.Equals("null"))
                .Select(a => long.Parse(a.DisciplinaCompartilhadaId)).Distinct();

            if (idsDisciplinasCompartilhadas != null && idsDisciplinasCompartilhadas.Any())
                idsDisciplinasAulas.AddRange(idsDisciplinasCompartilhadas);

            IEnumerable<DisciplinaDto> disciplinasEol = new List<DisciplinaDto>();
            if (idsDisciplinasAulas != null && idsDisciplinasAulas.Any())
                disciplinasEol = servicoEOL.ObterDisciplinasPorIds(idsDisciplinasAulas.ToArray());

            IEnumerable<DisciplinaResposta> disciplinasRegencia = Enumerable.Empty<DisciplinaResposta>();

            var disciplinaRegencia = disciplinasEol.FirstOrDefault(c => c.Regencia);
            if (temTurmaInformada && disciplinaRegencia != null)
            {
                if (usuario.EhProfessorCj())
                    disciplinasRegencia = await consultasDisciplina.ObterComponentesCJ(null,
                                                                                       filtro.TurmaId,
                                                                                       string.Empty,
                                                                                       disciplinaRegencia.CodigoComponenteCurricular,
                                                                                       usuario.CodigoRf);
                else
                    disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(filtro.TurmaId), usuario.CodigoRf, usuario.PerfilAtual);
            }

            aulas
            .ToList()
            .ForEach(x =>
            {
                bool podeCriarAtividade = true;
                var listaAtividades = atividades.Where(w => w.DataAvaliacao.Date == x.DataAula.Date && w.TurmaId == x.TurmaId
                && PossuiDisciplinas(w.Id, x.DisciplinaId)).ToList();
                var disciplina = disciplinasEol?.FirstOrDefault(d => d.CodigoComponenteCurricular.ToString().Equals(x.DisciplinaId));
                var disciplinaCompartilhada = disciplinasEol?.FirstOrDefault(d => d.CodigoComponenteCurricular.ToString().Equals(x.DisciplinaCompartilhadaId));
                if (atividades != null && disciplina != null)
                {
                    foreach (var item in listaAtividades)
                    {
                        if (disciplina.Regencia)
                        {
                            var disciplinasRegenciasComAtividades = repositorioAtividadeAvaliativaRegencia.Listar(item.Id).Result;

                            disciplinasRegenciasComAtividades.ToList().ForEach(r => r.DisciplinaContidaRegenciaNome = disciplinasRegencia?.FirstOrDefault(d => d.CodigoComponenteCurricular.ToString().Equals(r.DisciplinaContidaRegenciaId)).Nome);

                            item.AtividadeAvaliativaRegencia = new List<AtividadeAvaliativaRegencia>();
                            item.AtividadeAvaliativaRegencia.AddRange(disciplinasRegenciasComAtividades);
                            if (temTurmaInformada)
                                podeCriarAtividade = disciplinasRegencia.Count() > disciplinasRegenciasComAtividades.Count();
                            else podeCriarAtividade = false;
                        }
                        else
                            podeCriarAtividade = false;
                    }
                }

                var turma = turmasAbrangencia.FirstOrDefault(t => t.CodigoTurma.Equals(x.TurmaId));

                eventosAulas.Add(new EventosAulasTipoDiaDto
                {
                    Id = x.Id,
                    TipoEvento = x.AulaCJ ? "CJ" : "Aula",
                    DadosAula = new DadosAulaDto
                    {
                        DisciplinaId = disciplina?.CodigoComponenteCurricular ?? null,
                        Disciplina = $"{(disciplina?.Nome ?? "Disciplina não encontrada")} {(x.TipoAula == TipoAula.Reposicao ? "(Reposição)" : "")} {(x.Status == EntidadeStatus.AguardandoAprovacao ? "- Aguardando aprovação" : "")}",
                        DisciplinaCompartilhadaId = disciplinaCompartilhada?.CodigoComponenteCurricular ?? null,
                        DisciplinaCompartilhada = $"{(disciplinaCompartilhada?.Nome ?? "Disciplina não encontrada")} ",
                        EhRegencia = disciplina.Regencia,
                        EhCompartilhada = disciplina.Compartilhada,
                        PermiteRegistroFrequencia = disciplina.RegistraFrequencia && !x.SomenteConsulta,
                        podeCadastrarAvaliacao = podeCriarAtividade,
                        Horario = x.DataAula.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                        Modalidade = turma?.Modalidade.GetAttribute<DisplayAttribute>().Name ?? "Modalidade",
                        Tipo = turma?.TipoEscola.GetAttribute<DisplayAttribute>().ShortName ?? "Escola",
                        Turma = x.TurmaNome,
                        UnidadeEscolar = x.UeNome,
                        Atividade = listaAtividades
                    }
                });
            });

            return new DiaEventoAula
            {
                EventosAulas = eventosAulas,
                Letivo = comandosDiasLetivos.VerificarSeDataLetiva(eventos, data)
            };
        }

        public async Task<IEnumerable<EventosAulasCalendarioDto>> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro)
        {
            List<DateTime> diasLetivos = new List<DateTime>();
            List<DateTime> diasNaoLetivos = new List<DateTime>();
            List<DateTime> totalDias = new List<DateTime>();

            if (!filtro.TodasTurmas && string.IsNullOrWhiteSpace(filtro.TurmaId))
                throw new NegocioException("É necessario informar uma turma para pesquisa");

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            string rf = usuario.TemPerfilGestaoUes() ? string.Empty : usuario.CodigoRf;

            var diasPeriodoEscolares = comandosDiasLetivos.BuscarDiasLetivos(filtro.TipoCalendarioId);
            var diasAulas = await repositorioAula.ObterAulas(filtro.TipoCalendarioId, filtro.TurmaId, filtro.UeId, rf);
            var eventos = repositorioEvento.ObterEventosPorTipoDeCalendarioDreUe(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId, filtro.EhEventoSme, true, usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());

            var diasEventosNaoLetivos = comandosDiasLetivos.ObterDias(eventos, diasNaoLetivos, EventoLetivo.Nao);
            var diasEventosLetivos = comandosDiasLetivos.ObterDias(eventos, diasLetivos, EventoLetivo.Sim);
            var aulas = ObterDias(diasAulas);

            diasEventosNaoLetivos.RemoveAll(x => !diasPeriodoEscolares.Contains(x));
            aulas.RemoveAll(x => !diasPeriodoEscolares.Contains(x));

            totalDias.AddRange(aulas);
            totalDias.AddRange(diasEventosLetivos);
            totalDias.AddRange(diasEventosNaoLetivos);

            return MapearParaDto(totalDias);
        }

        public async Task<IEnumerable<EventosAulasTipoCalendarioDto>> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro)
        {
            if (!filtro.TodasTurmas && string.IsNullOrWhiteSpace(filtro.TurmaId))
                throw new NegocioException("É necessario informar uma turma para pesquisa");

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            string rf = usuario.TemPerfilGestaoUes() ? string.Empty : usuario.CodigoRf;

            var eventosAulas = new List<EventosAulasTipoCalendarioDto>();

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(filtro.TipoCalendarioId);

            if (periodoEscolar is null || !periodoEscolar.Any())
                throw new NegocioException($"Não existe periodo escolar cadastrado para o tipo de calendario de id {filtro.TipoCalendarioId}");

            var ano = periodoEscolar.FirstOrDefault().PeriodoInicio.Year;

            var aulas = await repositorioAula.ObterAulas(filtro.TipoCalendarioId, filtro.TurmaId, filtro.UeId, rf, filtro.Mes);
            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeMes(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId, filtro.Mes, filtro.EhEventoSme, usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());
            var atividadesAvaliativas = await repositorioAtividadeAvaliativa.ObterAtividadesPorMes(filtro.DreId, filtro.UeId, filtro.Mes, ano, rf, filtro.TurmaId);
            var diasAulas = ObterDiasAulas(aulas);
            var diasEventos = ObterDiasEventos(eventos, filtro.Mes);
            var diasAtividade = ObterDiasAtividades(atividadesAvaliativas);

            diasAulas.AddRange(diasEventos);
            diasAulas.AddRange(diasAtividade);
            return MapearParaDtoTipo(eventosAulas, diasAulas);
        }

        private static IEnumerable<EventosAulasTipoCalendarioDto> MapearParaDtoTipo(List<EventosAulasTipoCalendarioDto> eventosAulas, List<KeyValuePair<int, string>> diasAulas)
        {
            foreach (var dia in diasAulas.Select(x => x.Key).Distinct())
            {
                var qtdEventosAulas = diasAulas.Where(x => x.Key == dia).Count();
                eventosAulas.Add(new EventosAulasTipoCalendarioDto
                {
                    Dia = dia,
                    QuantidadeDeEventosAulas = qtdEventosAulas,
                    TemAtividadeAvaliativa = diasAulas.Where(x => x.Key == dia && x.Value == "Atividade avaliativa").Any(),
                    TemAula = diasAulas.Where(x => x.Key == dia && x.Value == "Aula").Any(),
                    TemAulaCJ = diasAulas.Where(x => x.Key == dia && x.Value == "CJ").Any(),
                    TemEvento = diasAulas.Where(x => x.Key == dia && x.Value == "Evento").Any()
                });
            }

            return eventosAulas.OrderBy(x => x.Dia);
        }

        private static void ObterAulasCompartilhadas(IEnumerable<AulaCompletaDto> aulas, List<AulaCompletaDto> aulasProfessor, IEnumerable<string> disciplinasCompartilhadas)
        {
            var aulasCompartilhadas = aulas.Where(x => !aulasProfessor.Any(y => y.Id == x.Id) && disciplinasCompartilhadas.Any(z => x.DisciplinaId.Equals(z)));

            aulasProfessor.AddRange(aulasCompartilhadas);
        }

        private static void ObterAulasCompartilhadasRelacionadas(IEnumerable<AulaCompletaDto> aulas, List<AulaCompletaDto> aulasProfessor)
        {
            var disciplinasPrincipais = aulasProfessor.Select(x => x.DisciplinaId);

            var aulasRelacionadas = aulas.Where(x => !aulasProfessor.Any(y => y.Id == x.Id) && disciplinasPrincipais.Any(z => z.Equals(x.DisciplinaCompartilhadaId)));

            if (aulasRelacionadas is null || !aulasRelacionadas.Any())
                return;

            aulasProfessor.AddRange(aulasRelacionadas);
        }

        private static void ObterEventosParaEventosAulasDia(List<EventosAulasTipoDiaDto> eventosAulas, IEnumerable<Evento> eventos)
        {
            eventos
                .ToList()
                .ForEach(x => eventosAulas
                .Add(new EventosAulasTipoDiaDto
                {
                    Descricao = x.Nome,
                    Id = x.Id,
                    TipoEvento = x.Descricao
                }));
        }

        private static void VerificarAulasSomenteConsulta(IEnumerable<DisciplinaResposta> disciplinas, IEnumerable<AulaCompletaDto> aulas)
        {
            aulas.ToList().ForEach(aula =>
            {
                var disciplina = disciplinas.FirstOrDefault(d
                    => d.CodigoComponenteCurricular.ToString().Equals(aula.DisciplinaId));

                var disciplinaId = disciplina == null ? "" : disciplina.CodigoComponenteCurricular.ToString();

                aula.VerificarSomenteConsulta(disciplinaId);
            });
        }

        private List<EventosAulasCalendarioDto> MapearParaDto(List<DateTime> dias)
        {
            List<EventosAulasCalendarioDto> eventosAulas = new List<EventosAulasCalendarioDto>();
            for (int mes = 1; mes <= 12; mes++)
            {
                eventosAulas.Add(new EventosAulasCalendarioDto
                {
                    Mes = mes,
                    EventosAulas = dias
                                    .Where(w => w.Month == mes)
                                    .Distinct()
                                    .Count()
                });
            }
            return eventosAulas;
        }

        private async Task<IEnumerable<AulaCompletaDto>> ObterAulasDia(FiltroEventosAulasCalendarioDiaDto filtro, DateTime data, Guid perfil, string professorRf, IEnumerable<DisciplinaResposta> disciplinas)
        {
            var aulas = await repositorioAula.ObterAulasCompleto(filtro.TipoCalendarioId, filtro.TurmaId, filtro.UeId, data, perfil, filtro.TurmaHistorico);

            if (disciplinas != null)
                VerificarAulasSomenteConsulta(disciplinas, aulas);

            if (string.IsNullOrWhiteSpace(professorRf))
                return aulas;

            var aulasProfessor = aulas.Where(x => !string.IsNullOrEmpty(x.ProfessorRF) && x.ProfessorRF.Equals(professorRf)).ToList();

            var disciplinasCompartilhadas = aulasProfessor.Where(x => !string.IsNullOrEmpty(x.DisciplinaCompartilhadaId)).Select(x => x.DisciplinaCompartilhadaId);

            if (disciplinasCompartilhadas.Any())
                ObterAulasCompartilhadas(aulas, aulasProfessor, disciplinasCompartilhadas);

            ObterAulasCompartilhadasRelacionadas(aulas, aulasProfessor);

            return aulasProfessor;
        }

        private List<DateTime> ObterDias(IEnumerable<AulaDto> aulas)
        {
            List<DateTime> dias = new List<DateTime>();
            dias.AddRange(aulas.Select(x => x.DataAula.Date));
            return dias.Distinct().ToList();
        }

        private List<KeyValuePair<int, string>> ObterDiasAtividades(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas)
        {
            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>();
            foreach (var atividade in atividadesAvaliativas)
            {
                dias.Add(new KeyValuePair<int, string>(atividade.DataAvaliacao.Day, "Atividade avaliativa"));
            }
            return dias;
        }

        private List<KeyValuePair<int, string>> ObterDiasAulas(IEnumerable<AulaDto> aulas)
        {
            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>();
            foreach (var aula in aulas)
            {
                dias.Add(new KeyValuePair<int, string>(aula.DataAula.Day, aula.AulaCJ ? "CJ" : "Aula"));
            }
            return dias;
        }

        private List<KeyValuePair<int, string>> ObterDiasEventos(IEnumerable<Dominio.Evento> eventos, int mes)
        {
            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>();
            foreach (var evento in eventos)
            {
                //se o evento ir para o próximo mês automaticamente ele já não irá nesse for
                for (DateTime dia = evento.DataInicio; dia <= evento.DataFim; dia = dia.AddDays(1))
                {
                    if (dia.Month != mes) break;
                    dias.Add(new KeyValuePair<int, string>(dia.Day, "Evento"));
                }
            }
            return dias;
        }

        private async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterTurmasAbrangencia(IEnumerable<string> turmasAulas, bool ehTurmaHistorico)
        {
            var turmasRetorno = new List<AbrangenciaFiltroRetorno>();

            foreach (var turma in turmasAulas)
            {
                var turmaAbrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(turma, ehTurmaHistorico);

                if (turmaAbrangencia != null)
                    turmasRetorno.Add(turmaAbrangencia);
            }

            return turmasRetorno;
        }

        private bool PossuiDisciplinas(long atividadeId, string disciplinaId)
        {
            return repositorioAtividadeAvaliativaDisciplina.PossuiDisciplinas(atividadeId, disciplinaId);
        }
    }
}