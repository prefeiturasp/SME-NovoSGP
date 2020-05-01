using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    [DisplayName("Frequência")]
    public class ServicoCalculoParecerFrequencia : ServicoCalculoParecerConclusivo, IServicoCalculoParecerFrequencia
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IServicoEOL servicoEOL;

        public ServicoCalculoParecerFrequencia(IRepositorioParametrosSistema repositorioParametrosSistema,
                                               IConsultasFrequencia consultasFrequencia,
                                               IServicoEOL servicoEOL)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));

            this.quandoVerdadeiro = Activator.CreateInstance<IServicoCalculoParecerNota>();
        }

        protected override IEnumerable<ConselhoClasseParecerConclusivo> FiltrarPareceresDoServico(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
            => pareceresDaTurma.Where(c => c.Frequencia);

        protected override async Task<bool> ValidarParecer(string alunoCodigo, string turmaCodigo)
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
    }
}
