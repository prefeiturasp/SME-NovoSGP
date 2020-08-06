using MediatR;
using SME.SGP.Aplicacao.Queries.Aula.ObterAulasDaTurma;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteUseCase : ICriarAulasInfantilAutomaticamenteUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        private readonly IUnitOfWork unitOfWork;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator, IRepositorioAula repositorioAula, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Executar()
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Infantil, anoAtual, null));
            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {
                    var diasLetivos = await mediator.Send(new ObterDiasLetivosPorPeriodosEscolaresQuery(periodosEscolares, tipoCalendarioId));

                    var diasParaCriarAula = diasLetivos?.Where(c => c.EhLetivo);
                    var diasParaExcluirAula = diasLetivos?.Where(c => !c.EhLetivo);

                    var turmas = await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));
                    if (turmas != null && turmas.Any())
                    {
                        var aulasACriar = new List<Aula>();
                        var aulasAExcluir = new List<Aula>();
                        foreach (var turma in turmas)
                        {
                            var aulas = await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId));
                            if (aulas == null)
                            {
                                aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula, turma));
                            }
                            else
                            {
                                if (!aulas.Any())
                                {
                                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula, turma));
                                }
                                else
                                {
                                    var diasSemAula = diasParaCriarAula.Where(c => !aulas.Any(a => a.DataAula == c.Data));
                                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasSemAula, turma));

                                    var aulasDaTurmaParaExcluir = aulas.Where(c => diasParaExcluirAula.Any(a => a.Data == c.DataAula));
                                    //TODO OBTER FREQUENCIA E DIARIO DE BORDO DAS AULAS PARA EXCLUIR
                                }
                            }
                        }
                        //TODO CRIAR
                        unitOfWork.IniciarTransacao();
                        foreach (var aula in aulasACriar)
                        {
                            await repositorioAula.SalvarAsync(aula);
                        }
                        unitOfWork.PersistirTransacao();
                    }
                }
            }
        }

        private static IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasParaCriarAula, Turma turma)
        {
            return diasParaCriarAula.Select(c => new Aula
            {
                DataAula = c.Data,
                DisciplinaId = "512",
                DisciplinaNome = "Regência de classe Infantil",
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = tipoCalendarioId,
                TurmaId = turma.CodigoTurma,
                UeId = turma.Ue.CodigoUe,
                ProfessorRf = "Sistema"
            });
        }
    }
}
