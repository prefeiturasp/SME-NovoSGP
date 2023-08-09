using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorIdUseCase : AbstractUseCase, IObterAulaPorIdUseCase
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly (string codigoAntiga, string codigoNova) disciplinasInglesAtualizacao = ("1046", "9");

        public ObterAulaPorIdUseCase(IMediator mediator, IConsultasDisciplina consultasDisciplina) : base(mediator)
        {
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AulaConsultaDto> Executar(long aulaId)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));

            if (aula == null || aula.Excluido)
                throw new NegocioException($"Aula de id {aulaId} não encontrada");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            
            var eventosDaUeSME = await mediator.Send(new ObterEventosCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = turma.Ue.CodigoUe,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                DataConsulta = aula.DataAula,
                TipoCalendarioId = aula.TipoCalendarioId
            });
            bool temEventoDeRecesso = false;
            eventosDaUeSME = eventosDaUeSME.Where(x => x.TipoEvento.Id == 11);
            if (eventosDaUeSME != null && eventosDaUeSME.Any())
                temEventoDeRecesso = true;

            var aberto = await AulaDentroDoPeriodo(aula.TurmaId, aula.DataAula);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var usuarioAcessoAoComponente = await UsuarioComAcessoAoComponente(usuarioLogado, aula, usuarioLogado.EhProfessorCj());

            var aulaEmManutencao = await mediator.Send(new ObterAulaEmManutencaoQuery(aula.Id));

            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(aula.DataAula, turma));

            bool temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));

            var compensacoesAusenciasAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery(aula.Id));
            if (compensacoesAusenciasAulas == null)
                compensacoesAusenciasAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>();

            return await MapearParaDto(aula, aberto, usuarioAcessoAoComponente, aulaEmManutencao, temPeriodoAberto, temEventoDeRecesso, compensacoesAusenciasAulas.Any());
        }

        private async Task<bool> AulaDentroDoPeriodo(string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(dataAula, turma));

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }

        private async Task<bool> UsuarioComAcessoAoComponente(Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            if (usuarioLogado.EhGestorEscolar())
                return true;

            var componentesUsuario = ehCJ
                                    ? await mediator.Send(new ObterComponentesCJQuery(null,
                                                                                      aula.TurmaId,
                                                                                      string.Empty,
                                                                                      long.Parse(aula.DisciplinaId),
                                                                                      usuarioLogado.CodigoRf, false))
                                    : await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(aula.TurmaId,
                                                                                                                    usuarioLogado.CodigoRf,
                                                                                                                    usuarioLogado.PerfilAtual));

            if ((aula.CriadoRF == usuarioLogado.CodigoRf) || aula.CriadoRF == "Sistema")
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));

                if (turma == null)
                    throw new NegocioException($"Turma de codigo [{aula.TurmaId}] não localizada!");

                var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(aula.TurmaId, turma.TipoTurma == TipoTurma.Programa);

                return disciplinas.Any(d => d.CodigoComponenteCurricular == long.Parse(aula.DisciplinaId)) ||
                       disciplinas.Any(d => d.CodigoTerritorioSaber == long.Parse(aula.DisciplinaId)) ||
                       disciplinas.Any(d => d.CdComponenteCurricularPai == long.Parse(aula.DisciplinaId));                
            }

            var disciplina = componentesUsuario?.FirstOrDefault(x => x.Codigo.ToString().Equals(aula.DisciplinaId));

            return disciplina != null;
        }

        private async Task<AulaConsultaDto> MapearParaDto(Aula aula, bool aberto, bool usuarioAcessoAoComponente, bool aulaEmManutencao, bool temPeriodoAberto, bool temEventoDeRecesso, bool possuiCompensacao)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            AulaConsultaDto dto = new AulaConsultaDto()
            {
                Id = aula.Id,
                DisciplinaId = aula.DisciplinaId.Equals(disciplinasInglesAtualizacao.codigoAntiga) ? await VerificaAtualizacaoComponenteIngles(aula.TurmaId) : aula.DisciplinaId,
                DisciplinaCompartilhadaId = aula.DisciplinaCompartilhadaId ?? "0",
                TurmaId = aula.TurmaId,
                UeId = aula.UeId,
                TipoCalendarioId = aula.TipoCalendarioId,
                DentroPeriodo = aberto,
                TipoAula = aula.TipoAula,
                Quantidade = aula.Quantidade,
                ProfessorRf = aula.ProfessorRf,
                DataAula = aula.DataAula.Local(),
                RecorrenciaAula = aula.RecorrenciaAula,
                AlteradoEm = aula.AlteradoEm,
                AlteradoPor = aula.AlteradoPor,
                AlteradoRF = aula.AlteradoRF,
                CriadoEm = aula.CriadoEm,
                CriadoPor = aula.CriadoPor,
                CriadoRF = aula.CriadoRF,
                Migrado = aula.Migrado,
                SomenteLeitura = !usuarioAcessoAoComponente || !temEventoDeRecesso ? !temPeriodoAberto : false,
                EmManutencao = aulaEmManutencao,
                PodeEditar = (usuarioLogado.EhProfessorCj() && aula.AulaCJ)
                          || (!aula.AulaCJ && (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar()))
                          || (!aula.AulaCJ && (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar() || usuarioLogado.EhProfessorPoed()
                          || usuarioLogado.EhProfessorPosl()))
                          || (usuarioLogado.EhProfessorPap() && aula.EhPAP),
                PossuiCompensacao = possuiCompensacao,
                AulaCJ = aula.AulaCJ
            };

            return dto;
        }

        private async Task<string> VerificaAtualizacaoComponenteIngles(string codigoTurma)
        {
            var componentesCurricularesTurma = await consultasDisciplina
                .ObterComponentesCurricularesPorProfessorETurma(codigoTurma, false);

            return componentesCurricularesTurma
                .Any(cc => cc.CodigoComponenteCurricular.ToString().Equals(disciplinasInglesAtualizacao.codigoAntiga)) ? 
                    disciplinasInglesAtualizacao.codigoAntiga : disciplinasInglesAtualizacao.codigoNova;
        }
    }
}
