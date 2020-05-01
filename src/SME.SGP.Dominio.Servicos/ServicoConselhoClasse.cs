using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasse : IServicoConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                     IRepositorioConselhoClasseParecerConclusivo repositorioParecer,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IConsultasConselhoClasse consultasConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(conselhoClasse.FechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Não foi possível localizar o fechamento da turma informado!");

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
            // Se não existir conselho de classe para o fechamento gera
            if (conselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(conselhoClasseAluno.ConselhoClasse);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(conselhoClasseAluno.ConselhoClasse);

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return (AuditoriaConselhoClasseAlunoDto)conselhoClasseAluno;
        }

        public async Task<ParecerConclusivoDto> GerarParecerConclusivoAlunoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var conselhoClasseAluno = await ObterConselhoClasseAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo);
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var pareceresDaTurma = await ObterPareceresDaTurma(turma.Id);

            var parecerConclusivo = await CalcularParecerConclusivoAluno(alunoCodigo, turma.CodigoTurma, pareceresDaTurma);
            conselhoClasseAluno.ConselhoClasseParecerId = parecerConclusivo.Id;
            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return new ParecerConclusivoDto() 
            { 
                Id = parecerConclusivo.Id,
                Nome = parecerConclusivo.Nome
            };
        }

        private async Task<ConselhoClasseParecerConclusivo> CalcularParecerConclusivoAluno(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
        {
            var servicoCalculoFrequencia = Activator.CreateInstance<IServicoCalculoParecerFrequencia>();

            return await servicoCalculoFrequencia.Calcular(alunoCodigo, turmaCodigo, pareceresDaTurma);
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
            ConselhoClasseAluno conselhoClasseAluno = null;
            if (conselhoClasseId == 0)
            {
                var conselhoClasse = new ConselhoClasse() { FechamentoTurmaId = fechamentoTurmaId };
                conselhoClasseAluno = new ConselhoClasseAluno() { AlunoCodigo = alunoCodigo, ConselhoClasse = conselhoClasse };

                await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasse.Id;
                await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
            }
            else
                conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAsync(conselhoClasseId, alunoCodigo);

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
