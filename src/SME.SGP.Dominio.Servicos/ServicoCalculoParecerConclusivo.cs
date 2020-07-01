using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoParecerConclusivo : IServicoCalculoParecerConclusivo
    {
        protected IServicoCalculoParecerConclusivo quandoVerdadeiro;
        protected IServicoCalculoParecerConclusivo quandoFalso;
        protected IEnumerable<ConselhoClasseParecerConclusivo> pareceresDoServico;

        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IServicoEol servicoEOL;

        public ServicoCalculoParecerConclusivo(IRepositorioParametrosSistema repositorioParametrosSistema,
                                               IRepositorioFechamentoNota repositorioFechamentoNota,
                                               IRepositorioConceito repositorioConceito,
                                               IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                               IConsultasFrequencia consultasFrequencia,
                                               IServicoEol servicoEOL)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public bool Filtrar(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDoServico, string nomeClasseCalculo)
        {
            this.pareceresDoServico = pareceresDoServico;

            // Verifica se retornou 1 verdadeiro e 1 falso
            if (pareceresDoServico == null || !pareceresDoServico.Any())
                return false;

            if (!pareceresDoServico.Where(c => c.Aprovado).Any())
                throw new NegocioException($"Não localizado parecer conclusivo aprovado para o calculo por {nomeClasseCalculo}");
            if (pareceresDoServico.Where(c => c.Aprovado).Count() > 1)
                throw new NegocioException($"Encontrado mais de 1 parecer conclusivo aprovado para o calculo por {nomeClasseCalculo}");

            if (!pareceresDoServico.Where(c => !c.Aprovado).Any())
                throw new NegocioException($"Não localizado parecer conclusivo reprovado para o calculo por {nomeClasseCalculo}");
            if (pareceresDoServico.Where(c => !c.Aprovado).Count() > 1)
                throw new NegocioException($"Encontrado mais de 1 parecer conclusivo reprovado para o calculo por {nomeClasseCalculo}");

            return true;
        }

        private ConselhoClasseParecerConclusivo ObterParecerValidacao(bool retornoValidacao)
            => pareceresDoServico.FirstOrDefault(c => c.Aprovado == retornoValidacao);

        public async Task<ConselhoClasseParecerConclusivo> Calcular(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
        {
            // Frequencia
            Filtrar(pareceresDaTurma.Where(c => c.Frequencia), "Frequência");
            if (!await ValidarParecerPorFrequencia(alunoCodigo, turmaCodigo))
                return ObterParecerValidacao(false);

            var parecerFrequencia = ObterParecerValidacao(true);

            // Nota
            if (!Filtrar(pareceresDaTurma.Where(c => c.Nota), "Nota"))
                return parecerFrequencia;

            var parecerNota = ObterParecerValidacao(await ValidarParecerPorNota(alunoCodigo, turmaCodigo));

            // Conselho
            if (!Filtrar(pareceresDaTurma.Where(c => c.Conselho), "Conselho"))
                return parecerNota;

            var validacaoConselho = await ValidarParecerPorConselho(alunoCodigo, turmaCodigo);
            if (!validacaoConselho.ExisteNotaConselho)
                return parecerNota;

            return ObterParecerValidacao(validacaoConselho.ValidacaoNotaConselho);
        }

        #region Frequência
        private async Task<bool> ValidarParecerPorFrequencia(string alunoCodigo, string turmaCodigo)
        {
            if (!await ValidarFrequenciaGeralAluno(alunoCodigo, turmaCodigo))
                return false;

            return await ValidarFrequenciaBaseNacionalAluno(alunoCodigo, turmaCodigo);
        }
        private async Task<bool> ValidarFrequenciaBaseNacionalAluno(string alunoCodigo, string turmaCodigo)
        {
            var parametroFrequenciaBaseNacional = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualFrequenciaCriticoBaseNacional));
            var componentesCurriculares = await servicoEOL.ObterDisciplinasPorCodigoTurma(turmaCodigo);
            // Filtra componentes da Base Nacional
            var componentesCurricularesBaseNacional = componentesCurriculares.Where(c => c.BaseNacional);
            foreach (var componenteCurricular in componentesCurricularesBaseNacional)
            {
                var frequenciaGeralComponente = await consultasFrequencia.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo, componenteCurricular.CodigoComponenteCurricular.ToString());
                if (frequenciaGeralComponente < parametroFrequenciaBaseNacional)
                    return false;
            }

            return true;
        }

        private async Task<bool> ValidarFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo)
        {
            var frequenciaAluno = await consultasFrequencia.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo);

            var parametroFrequenciaGeral = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualFrequenciaCritico));
            return !(frequenciaAluno < parametroFrequenciaGeral);
        }
        #endregion

        #region Nota
        private async Task<bool> ValidarParecerPorNota(string alunoCodigo, string turmaCodigo)
        {
            var notasFechamentoAluno = await repositorioFechamentoNota.ObterNotasFinaisAlunoAsync(turmaCodigo, alunoCodigo);
            if (notasFechamentoAluno == null || !notasFechamentoAluno.Any())
                return true;

            var tipoNota = notasFechamentoAluno.First().ConceitoId.HasValue ? TipoNota.Conceito : TipoNota.Nota;
            return tipoNota == TipoNota.Nota ?
                ValidarParecerPorNota(notasFechamentoAluno) :
                await ValidarParecerPorConceito(notasFechamentoAluno);
        }

        private bool ValidarParecerPorNota(IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            foreach (var notaFechamentoAluno in notasFechamentoAluno)
                if (notaFechamentoAluno.Nota < notaMedia)
                    return false;

            return true;
        }

        private async Task<bool> ValidarParecerPorConceito(IEnumerable<NotaConceitoBimestreComponenteDto> conceitosFechamentoAluno)
        {
            var conceitosVigentes = await repositorioConceito.ObterPorData(DateTime.Today);
            foreach (var conceitoFechamentoAluno in conceitosFechamentoAluno)
            {
                var conceitoAluno = conceitosVigentes.FirstOrDefault(c => c.Id == conceitoFechamentoAluno.ConceitoId);
                if (!conceitoAluno.Aprovado)
                    return false;
            }

            return true;
        }
        #endregion

        #region Conselho
        private async Task<(bool ExisteNotaConselho, bool ValidacaoNotaConselho)> ValidarParecerPorConselho(string alunoCodigo, string turmaCodigo)
        {
            var notasConselhoClasse = await repositorioConselhoClasseNota.ObterNotasConselhoAlunoAsync(alunoCodigo, turmaCodigo, null);
            if (notasConselhoClasse == null || !notasConselhoClasse.Any())
                return (false, false);

            var tipoNota = notasConselhoClasse.First().ConceitoId.HasValue ? TipoNota.Conceito : TipoNota.Nota;
            return (true, tipoNota == TipoNota.Nota ?
                ValidarParecerConselhoPorNota(notasConselhoClasse) :
               await ValidarParecerConselhoPorConceito(notasConselhoClasse));
        }

        private bool ValidarParecerConselhoPorNota(IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasse)
        {
            var notaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            foreach (var notaConcelhoClasse in notasConselhoClasse)
                if (notaConcelhoClasse.Nota < notaMedia)
                    return false;

            return true;
        }

        private async Task<bool> ValidarParecerConselhoPorConceito(IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasse)
        {
            var conceitosVigentes = await repositorioConceito.ObterPorData(DateTime.Today);
            foreach (var conceitoConselhoClasseAluno in notasConselhoClasse)
            {
                var conceitoAluno = conceitosVigentes.FirstOrDefault(c => c.Id == conceitoConselhoClasseAluno.ConceitoId);
                if (!conceitoAluno.Aprovado)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
