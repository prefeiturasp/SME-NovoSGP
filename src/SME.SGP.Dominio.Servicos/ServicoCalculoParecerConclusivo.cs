using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public abstract class ServicoCalculoParecerConclusivo : IServicoCalculoParecerConclusivo
    {
        protected IServicoCalculoParecerConclusivo quandoVerdadeiro;
        protected IServicoCalculoParecerConclusivo quandoFalso;
        protected IEnumerable<ConselhoClasseParecerConclusivo> pareceresDoServico;

        public ServicoCalculoParecerConclusivo()
        {
        }

        protected abstract Task<bool> ValidarParecer(string alunoCodigo, string turmaCodigo);
        protected abstract IEnumerable<ConselhoClasseParecerConclusivo> FiltrarPareceresDoServico(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma);

        public async Task Filtrar(IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
        {
            pareceresDoServico = FiltrarPareceresDoServico(pareceresDaTurma);

            var displayNameProperty = this.GetType()
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault() as DisplayNameAttribute;
            var nomeClasseCalculo = displayNameProperty.DisplayName;

            // Verifica se retornou 1 verdadeiro e 1 falso
            if (pareceresDoServico == null || !pareceresDoServico.Any())
                throw new NegocioException($"Não localizado pareceres conclusivos na base para o calculo por {nomeClasseCalculo}");

            if (!pareceresDoServico.Where(c => c.Aprovado).Any())
                throw new NegocioException($"Não localizado parecer conclusivo aprovado para o calculo por {nomeClasseCalculo}");
            if (pareceresDoServico.Where(c => c.Aprovado).Count() > 1)
                throw new NegocioException($"Encontrado mais de 1 parecer conclusivo aprovado para o calculo por {nomeClasseCalculo}");

            if (!pareceresDoServico.Where(c => !c.Aprovado).Any())
                throw new NegocioException($"Não localizado parecer conclusivo reprovado para o calculo por {nomeClasseCalculo}");
            if (pareceresDoServico.Where(c => !c.Aprovado).Count() > 1)
                throw new NegocioException($"Encontrado mais de 1 parecer conclusivo reprovado para o calculo por {nomeClasseCalculo}");
        }

        public async Task<ConselhoClasseParecerConclusivo> Calcular(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
        {
            await Filtrar(pareceresDaTurma);
            var retornoValidacao = await ValidarParecer(alunoCodigo, turmaCodigo);

            if (retornoValidacao && (quandoVerdadeiro != null))
                return await quandoVerdadeiro.Calcular(alunoCodigo, turmaCodigo, pareceresDaTurma);
            else
            if (!retornoValidacao && (quandoFalso != null))
                return await quandoFalso.Calcular(alunoCodigo, turmaCodigo, pareceresDaTurma);
            else
                return await ObterParecerValidacao(retornoValidacao);

        }

        private async Task<ConselhoClasseParecerConclusivo> ObterParecerValidacao(bool retornoValidacao)
            => pareceresDoServico.FirstOrDefault(c => c.Aprovado == retornoValidacao);
    }
}
