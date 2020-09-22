using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasse : IServicoConselhoClasse
    {
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDre repositorioDre;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoCalculoParecerConclusivo servicoCalculoParecerConclusivo;
        private readonly IServicoEol servicoEOL;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IRepositorioConselhoClasseParecerConclusivo repositorioParecer,
                                     IRepositorioTipoCalendario repositorioTipoCalendario,
                                     IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                     IRepositorioUe repositorioUe,
                                     IRepositorioDre repositorioDre,
                                     IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                     IConsultasTurma consultasTurma,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IConsultasConselhoClasse consultasConselhoClasse,
                                     IConsultasDisciplina consultasDisciplina,
                                     IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                     IUnitOfWork unitOfWork,
                                     IServicoCalculoParecerConclusivo servicoCalculoParecerConclusivo,
                                     IServicoEol servicoEOL)

        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoCalculoParecerConclusivo = servicoCalculoParecerConclusivo ?? throw new ArgumentNullException(nameof(servicoCalculoParecerConclusivo));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<ConselhoClasseNotaRetornoDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre)
        {
            var conselhoClasseNota = new ConselhoClasseNota();

            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();

            if (fechamentoTurma == null)
            {
                var turma = await consultasTurma.ObterPorCodigo(codigoTurma);
                if (turma == null) throw new NegocioException("Turma não encontrada");

                var ue = repositorioUe.ObterPorId(turma.UeId);
                ue.AdicionarDre(repositorioDre.ObterPorId(ue.DreId));
                turma.AdicionarUe(ue);               

                if (turma.AnoLetivo == DateTime.Today.Year) throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
                if (tipoCalendario == null) throw new NegocioException("Tipo calendário não encontrado");

                var periodoEscolar = await consultasPeriodoEscolar.ObterPeriodoEscolarPorTipoCalendarioBimestre(tipoCalendario.Id, bimestre);

                fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = turma.Id,
                    Migrado = false,
                    PeriodoEscolarId = periodoEscolar?.Id,
                    Turma = turma,
                    PeriodoEscolar = periodoEscolar
                };

                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = conselhoClasseNotaDto.CodigoComponenteCurricular
                };

            }
            try
            {
                unitOfWork.IniciarTransacao();
                if (fechamentoTurmaDisciplina.DisciplinaId > 0)
                {
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurma);
                    fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurma.Id;
                    await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                }
                if (fechamentoTurma.PeriodoEscolarId.HasValue)
                {
                    // Fechamento Bimestral
                    if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                        throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
                }
                else
                {
                    // Fechamento Final
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2 && fechamentoTurma.Turma.AnoLetivo == DateTime.Today.Year)
                        throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
                unitOfWork.PersistirTransacao();
            }
            catch(Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }

            try
            {
                if (conselhoClasseId == 0)
                {

                    var conselhoClasse = new ConselhoClasse();
                    conselhoClasse.FechamentoTurmaId = fechamentoTurma.Id;

                    unitOfWork.IniciarTransacao();
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                    conselhoClasseId = conselhoClasse.Id;

                    long conselhoClasseAlunoId = await SalvarConselhoClasseAlunoResumido(conselhoClasse.Id, alunoCodigo);

                    conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                    unitOfWork.PersistirTransacao();
                }
                else
                {
                    var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
                    unitOfWork.IniciarTransacao();

                    var conselhoClasseAlunoId = conselhoClasseAluno != null ? conselhoClasseAluno.Id : await SalvarConselhoClasseAlunoResumido(conselhoClasseId, alunoCodigo);

                    conselhoClasseNota = await repositorioConselhoClasseNota.ObterPorConselhoClasseAlunoComponenteCurricularAsync(conselhoClasseAlunoId, conselhoClasseNotaDto.CodigoComponenteCurricular);

                    if (conselhoClasseNota == null)
                    {
                        conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);
                    }
                    else
                    {
                        conselhoClasseNota.Justificativa = conselhoClasseNotaDto.Justificativa;
                        if (conselhoClasseNotaDto.Nota.HasValue)
                            conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
                        else conselhoClasseNota.Nota = null;
                        if (conselhoClasseNotaDto.Conceito.HasValue)
                            conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;
                    }

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);

                    unitOfWork.PersistirTransacao();
                }
            }

            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
            var auditoria = (AuditoriaDto)conselhoClasseNota;
            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurma.Id,
                Auditoria = auditoria
            };
            return conselhoClasseNotaRetorno;
        }

        private async Task<long> SalvarConselhoClasseAlunoResumido(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAluno = new ConselhoClasseAluno()
            {
                AlunoCodigo = alunoCodigo,
                ConselhoClasseId = conselhoClasseId
            };

            return await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
        }

        private ConselhoClasseNota ObterConselhoClasseNota(ConselhoClasseNotaDto conselhoClasseNotaDto, long conselhoClasseAlunoId)
        {
            var conselhoClasseNota = new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                ComponenteCurricularCodigo = conselhoClasseNotaDto.CodigoComponenteCurricular,
                Justificativa = conselhoClasseNotaDto.Justificativa,
            };
            if (conselhoClasseNotaDto.Nota.HasValue)
                conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
            if (conselhoClasseNotaDto.Conceito.HasValue)
                conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;
            return conselhoClasseNota;
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse, FechamentoTurma fechamentoTurma)
        {
            var conselhoClasseExistente = await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId);
            if (conselhoClasseExistente != null)
                throw new NegocioException($"Já existe um conselho de classe gerado para a turma {fechamentoTurma.Turma.Nome}!");

            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                if (!validacaoConselhoFinal.Item2)
                    throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
        }

        public async Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            var fechamentoTurma = await ObterFechamentoTurma(conselhoClasseAluno.ConselhoClasse.FechamentoTurmaId);
            if (!await VerificaNotasTodosComponentesCurriculares(conselhoClasseAluno.AlunoCodigo, fechamentoTurma.Turma, fechamentoTurma.PeriodoEscolarId))
                throw new NegocioException("É necessário que todos os componentes tenham nota/conceito informados!");

            // Se não existir conselho de classe para o fechamento gera
            if (conselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(conselhoClasseAluno.ConselhoClasse, fechamentoTurma);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(conselhoClasseAluno.ConselhoClasse);

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return (AuditoriaConselhoClasseAlunoDto)conselhoClasseAluno;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId)
        {
            var notasAluno = await repositorioConselhoClasseNota.ObterNotasAlunoAsync(alunoCodigo, turma.CodigoTurma, periodoEscolarId);

            var componentesCurriculares = await ObterComponentesTurma(turma);

            // Checa se todas as disciplinas da turma receberam nota
            foreach (var componenteCurricular in componentesCurriculares.Where(c => c.LancaNota))
                if (!notasAluno.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular))
                    return false;

            return true;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurma(Turma turma)
        {
            var componentesTurma = new List<DisciplinaDto>();
            var componentesCurriculares = await consultasDisciplina.ObterDisciplinasPorTurma(turma.CodigoTurma, false);
            if (componentesCurriculares == null)
                throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

            componentesTurma.AddRange(componentesCurriculares.Where(c => !c.Regencia));
            foreach (var componenteCurricular in componentesCurriculares.Where(c => c.Regencia))
            {
                // Adiciona lista de componentes relacionados a regencia
                componentesTurma.AddRange(
                    consultasDisciplina.MapearParaDto(
                        await consultasDisciplina.ObterComponentesRegencia(turma, componenteCurricular.CodigoComponenteCurricular)));
            }

            return componentesTurma;
        }

        public async Task<ParecerConclusivoDto> GerarParecerConclusivoAlunoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var conselhoClasseAluno = await ObterConselhoClasseAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo);
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;

            // Se não possui notas de fechamento nem de conselho retorna um Dto vazio
            if (!await VerificaNotasTodosComponentesCurriculares(alunoCodigo, turma, null))
                return new ParecerConclusivoDto();

            var pareceresDaTurma = await ObterPareceresDaTurma(turma.Id);

            var parecerConclusivo = await servicoCalculoParecerConclusivo.Calcular(alunoCodigo, turma.CodigoTurma, pareceresDaTurma);
            conselhoClasseAluno.ConselhoClasseParecerId = parecerConclusivo.Id;
            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return new ParecerConclusivoDto()
            {
                Id = parecerConclusivo.Id,
                Nome = parecerConclusivo.Nome
            };
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(long turmaId)
        {
            var pareceresConclusivos = await repositorioParecer.ObterListaPorTurmaIdAsync(turmaId, DateTime.Today);
            if (pareceresConclusivos == null || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
        }

        private async Task<ConselhoClasseAluno> ObterConselhoClasseAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            ConselhoClasseAluno conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (conselhoClasseAluno == null)
            {
                ConselhoClasse conselhoClasse = null;
                if (conselhoClasseId == 0)
                {
                    conselhoClasse = new ConselhoClasse() { FechamentoTurmaId = fechamentoTurmaId };
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
                }
                else
                    conselhoClasse = repositorioConselhoClasse.ObterPorId(conselhoClasseId);

                conselhoClasseAluno = new ConselhoClasseAluno() { AlunoCodigo = alunoCodigo, ConselhoClasse = conselhoClasse, ConselhoClasseId = conselhoClasse.Id };
                await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
            }
            conselhoClasseAluno.ConselhoClasse.FechamentoTurma = await ObterFechamentoTurma(fechamentoTurmaId);

            return conselhoClasseAluno;
        }

        private async Task<FechamentoTurma> ObterFechamentoTurma(long fechamentoTurmaId)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Fechamento da turma não localizado");

            return fechamentoTurma;
        }
    }
}