using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAulaPrevista : IConsultasAulaPrevista
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta;
        private readonly IRepositorioAulaPrevistaBimestreConsulta repositorioBimestre;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IMediator mediator;

        private const string CODIGO_DISCIPLINA_INGLES = "9";
        private const string CODIGO_ALTERNATIVO_DISCIPLINA_INGLES = "1046";

        public ConsultasAulaPrevista(IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta,
                                     IRepositorioAulaPrevistaBimestreConsulta repositorioBimestre,
                                     IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                     IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                     IConsultasTurma consultasTurma,
                                     IMediator mediator)
        {
            this.repositorioAulaPrevistaConsulta = repositorioAulaPrevistaConsulta ?? throw new ArgumentNullException(nameof(repositorioAulaPrevistaConsulta));
            this.repositorioBimestre = repositorioBimestre ?? throw new ArgumentNullException(nameof(repositorioBimestre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AulasPrevistasDadasAuditoriaDto> BuscarPorId(long id)
        {
            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = null;
            var aulaPrevista = repositorioAulaPrevistaConsulta.ObterPorId(id);

            if (aulaPrevista != null)
            {
                var aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id);
                aulaPrevistaDto = MapearDtoRetorno(aulaPrevista, aulaPrevistaBimestres);
            }

            return aulaPrevistaDto;
        }

        public async Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId, int semestre = 0)
        {
            var turma = await ObterTurma(turmaId);
            var tipoCalendario = await ObterTipoCalendarioPorTurmaAnoLetivo(turma.AnoLetivo, turma.ModalidadeCodigo, semestre);
            var totalAulasPrevistas = await mediator.Send(new ObterAulasPrevistasPorCodigoUeQuery(turma.UeId));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var codigoTerritorioCorrespondente = (long?)null;

            var rf = usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME() || usuarioLogado.EhProfessor() ? string.Empty : usuarioLogado.CodigoRf;

            if (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar())
            {
                var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));
                var componenteCorrespondente = componentesProfessor.FirstOrDefault(cp => cp.Codigo.ToString().Equals(disciplinaId) || cp.CodigoComponenteTerritorioSaber.ToString().Equals(disciplinaId));
                codigoTerritorioCorrespondente = componenteCorrespondente != null && componenteCorrespondente.TerritorioSaber && componenteCorrespondente.Codigo.ToString().Equals(disciplinaId) ? componenteCorrespondente?.CodigoComponenteTerritorioSaber : componenteCorrespondente?.Codigo;
                if (usuarioLogado.EhGestorEscolar())
                    rf = componenteCorrespondente.Professor;
            }
            else if (usuarioLogado.EhProfessorCj())
            {
                var professores = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turma.Id));
                var professor = professores.FirstOrDefault(p => p.DisciplinasId.Contains(long.Parse(disciplinaId)));
                if (professor != null)
                {
                    rf = professor.ProfessorRf;
                    var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, professor.ProfessorRf, Perfis.PERFIL_PROFESSOR));
                    var componenteProfessorAtrelado = componentesProfessor.FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.ToString().Equals(disciplinaId));
                    codigoTerritorioCorrespondente = componenteProfessorAtrelado?.Codigo ?? null;
                }
            }
            
            var aulaPrevista = codigoTerritorioCorrespondente.HasValue ?
                    totalAulasPrevistas.FirstOrDefault(x => x.TipoCalendarioId == tipoCalendario.Id && x.TurmaId == turma.CodigoTurma && x.CriadoRF.Equals(rf) && (x.DisciplinaId == disciplinaId || x.DisciplinaId == codigoTerritorioCorrespondente.Value.ToString())) :
                    totalAulasPrevistas.FirstOrDefault(x => x.TipoCalendarioId == tipoCalendario.Id && x.TurmaId == turma.CodigoTurma && x.DisciplinaId == disciplinaId);

            if (aulaPrevista == null)
                aulaPrevista = await repositorioAulaPrevistaConsulta.ObterAulaPrevistaFiltro(tipoCalendario.Id, turmaId, disciplinaId.Equals(CODIGO_DISCIPLINA_INGLES) ? CODIGO_ALTERNATIVO_DISCIPLINA_INGLES : disciplinaId);

            var ehAnoLetivo = turma.AnoLetivo == DateTime.Today.Year;
            var periodosAbertos = await consultasTurma
                .PeriodosEmAbertoTurma(turmaId, DateTime.Now, ehAnoLetivo);

            IEnumerable<AulaPrevistaBimestreQuantidade> aulaPrevistaBimestres;

            if (aulaPrevista != null)
                aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id, codigoTerritorioCorrespondente?.ToString() ?? null, rf);
            else
            {
                totalAulasPrevistas = await mediator.Send(new ObterAulasPrevistasPorCodigoUeQuery(turma.UeId, false));
                aulaPrevista = codigoTerritorioCorrespondente.HasValue ?
                    totalAulasPrevistas.FirstOrDefault(x => x.TipoCalendarioId == tipoCalendario.Id && x.TurmaId == turma.CodigoTurma && x.CriadoRF.Equals(usuarioLogado.CriadoRF) && (x.DisciplinaId == disciplinaId || x.DisciplinaId == codigoTerritorioCorrespondente.Value.ToString())) :
                    totalAulasPrevistas.FirstOrDefault(x => x.TipoCalendarioId == tipoCalendario.Id && x.TurmaId == turma.CodigoTurma && x.DisciplinaId == disciplinaId);

                if (aulaPrevista == null)
                {
                    aulaPrevista = new AulaPrevista();
                    var periodosBimestre = await ObterPeriodosEscolares(tipoCalendario.Id);
                    aulaPrevistaBimestres = MapearPeriodoParaBimestreDto(periodosBimestre);
                }
                else
                    aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id, codigoTerritorioCorrespondente?.ToString() ?? null);
            }

            return MapearDtoRetorno(aulaPrevista, aulaPrevistaBimestres, periodosAbertos);
        }

        public async Task<int> ObterAulasDadas(Turma turma, string componenteCurricularCodigo, int bimestre)
        {
            var aulaPrevisa = await ObterAulaPrevistaDada(turma.ModalidadeCodigo, turma.CodigoTurma, componenteCurricularCodigo, turma.Semestre);

            return aulaPrevisa.AulasPrevistasPorBimestre.FirstOrDefault(a => a.Bimestre == bimestre)?.Cumpridas ?? 0;
        }

        private AulasPrevistasDadasAuditoriaDto MapearDtoRetorno(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> aulasPrevistasBimestre, IEnumerable<PeriodoEscolarAbertoDto> periodosAbertos = null)
        {
            if (aulasPrevistasBimestre.Any())
                aulasPrevistasBimestre = aulasPrevistasBimestre.DistinctBy(a => a.Bimestre).ToList();

            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = MapearParaDto(aulaPrevista, aulasPrevistasBimestre, periodosAbertos) ?? new AulasPrevistasDadasAuditoriaDto();
            aulaPrevistaDto = MapearMensagens(aulaPrevistaDto);

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearMensagens(AulasPrevistasDadasAuditoriaDto aulaPrevistaDto)
        {
            if (aulaPrevistaDto.AulasPrevistasPorBimestre != null)
            {
                foreach (var aula in aulaPrevistaDto.AulasPrevistasPorBimestre)
                {
                    List<string> mensagens = new List<string>();

                    if (aula.Previstas.Quantidade != (aula.Criadas.QuantidadeCJ + aula.Criadas.QuantidadeTitular) && aula.Fim.Date >= DateTime.Today)
                        mensagens.Add("Quantidade de aulas previstas diferente da quantidade de aulas criadas.");

                    if (aula.Previstas.Quantidade != (aula.Cumpridas + aula.Reposicoes) && aula.Fim.Date < DateTime.Today)
                        mensagens.Add("Quantidade de aulas previstas diferente do somatório de aulas dadas + aulas repostas, após o final do bimestre.");

                    if (mensagens.Any())
                        aula.Previstas.Mensagens = mensagens.ToArray();
                }

            }

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearParaDto(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> bimestres = null, IEnumerable<PeriodoEscolarAbertoDto> periodosAbertos = null)
        {
            var bimestre = bimestres.FirstOrDefault();

            return aulaPrevista == null ? null : new AulasPrevistasDadasAuditoriaDto
            {
                Id = aulaPrevista.Id,
                AlteradoEm = bimestre?.AlteradoEm ?? DateTime.MinValue,
                AlteradoPor = bimestre?.AlteradoPor ?? "",
                AlteradoRF = bimestre?.AlteradoRF ?? "",
                CriadoEm = bimestre?.CriadoEm ?? aulaPrevista.CriadoEm,
                CriadoPor = bimestre?.CriadoPor ?? aulaPrevista.CriadoPor,
                CriadoRF = bimestre?.CriadoRF ?? aulaPrevista.CriadoRF,
                AulasPrevistasPorBimestre = bimestres?.Select(x => new AulasPrevistasDadasDto
                {
                    Bimestre = x.Bimestre,
                    Criadas = new AulasQuantidadePorProfessorDto()
                    {
                        QuantidadeCJ = x.CriadasCJ,
                        QuantidadeTitular = x.CriadasTitular
                    },
                    Cumpridas = x.LancaFrequencia || x.Cumpridas > 0 ? x.Cumpridas : x.CumpridasSemFrequencia,
                    Inicio = x.Inicio,
                    Fim = x.Fim,
                    Previstas = new AulasPrevistasDto() { Quantidade = x.Previstas },
                    Reposicoes = x.LancaFrequencia || x.Reposicoes != 0 ? x.Reposicoes : x.ReposicoesSemFrequencia,
                    PodeEditar = periodosAbertos != null ? periodosAbertos.FirstOrDefault(p => p.Bimestre == x.Bimestre).Aberto : false
                }).ToList()
            };
        }

        private IEnumerable<AulaPrevistaBimestreQuantidade> MapearPeriodoParaBimestreDto(IEnumerable<PeriodoEscolar> periodoEscolares)
        {
            return periodoEscolares?.Select(x => new AulaPrevistaBimestreQuantidade
            {
                Bimestre = x.Bimestre,
                Inicio = x.PeriodoInicio,
                Fim = x.PeriodoFim
            }).ToList();
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }

        private async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestres(long? aulaPrevistaId, string disciplinaIdConsiderada = null, string professor = null)
        {
            return await repositorioBimestre.ObterBimestresAulasPrevistasPorId(aulaPrevistaId, disciplinaIdConsiderada, professor);
        }

        private async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEscolares(long tipoCalendarioId)
        {
            return await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
        }

        private async Task<TipoCalendario> ObterTipoCalendarioPorTurmaAnoLetivo(int anoLetivo, Modalidade turmaModalidade, int semestre)
        {
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeParaModalidadeTipoCalendario(turmaModalidade), semestre);

            if (tipoCalendario == null)
                throw new NegocioException("Tipo calendário não encontrado!");

            return tipoCalendario;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            return turma;
        }
    }
}