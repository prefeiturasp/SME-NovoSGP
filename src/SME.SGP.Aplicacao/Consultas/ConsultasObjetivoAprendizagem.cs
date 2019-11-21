using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagem : IConsultasObjetivoAprendizagem
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IServicoJurema servicoJurema;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema,
                                             IRepositorioCache repositorioCache,
                                             IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                             IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                             IConfiguration configuration)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }

        public async Task<bool> DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina)
        {
            var objetivos = await Listar();
            if (objetivos == null)
            {
                throw new NegocioException("Não foi possível obter a lista de objetivos de aprendizagem");
            }
            IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();
            var componentesFiltro = componentesCurriculares.Where(c => c.CodigoEOL == codigoDisciplina);
            var componentesJurema = componentesFiltro?.Select(c => c.CodigoJurema);
            if (componentesJurema == null)
            {
                return false;
            }
            return objetivos.Any(c => componentesJurema.Contains(c.IdComponenteCurricular));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Filtrar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            var objetivos = await Listar();
            var componentesJurema = ObterComponentesJuremaPorIdEOL(filtroObjetivosAprendizagemDto.ComponentesCurricularesIds);

            return objetivos?
                .Where(c => componentesJurema.Contains(c.IdComponenteCurricular)
                    && c.Ano == filtroObjetivosAprendizagemDto.Ano);
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Listar()
        {
            List<ObjetivoAprendizagemDto> objetivos;

            var objetivosCacheString = repositorioCache.Obter("ObjetivosAprendizagem");

            if (string.IsNullOrEmpty(objetivosCacheString))
            {
                var objetivosJuremaDto = await servicoJurema.ObterListaObjetivosAprendizagem();
                objetivos = MapearParaDto(objetivosJuremaDto).ToList();

                var tempoExpiracao = int.Parse(configuration.GetSection("ExpiracaoCache").GetSection("ObjetivosAprendizagem").Value);

                await repositorioCache.SalvarAsync("ObjetivosAprendizagem", JsonConvert.SerializeObject(objetivos), tempoExpiracao);
            }
            else
                objetivos = JsonConvert.DeserializeObject<List<ObjetivoAprendizagemDto>>(objetivosCacheString);

            return objetivos;
        }

        public async Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(int ano, int bimestre, long turmaId, long componenteCurricularId)
        {
            return repositorioObjetivosPlano.ObterDisciplinasDoBimestrePlanoAula(ano, bimestre, turmaId, componenteCurricularId);
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId)
        {
            var objetivosPlano = repositorioObjetivosPlano.ObterObjetivosPlanoDisciplina(ano, bimestre, turmaId, componenteCurricularId, disciplinaId);

            var objetivosJurema = await Listar();

            // filtra objetivos do jurema com os objetivos cadastrados no plano anual nesse bimestre
            return objetivosJurema.
                Where(c => objetivosPlano.Any(o => o.ObjetivoAprendizagemJuremaId == c.Id));
        }

        private IEnumerable<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagemResposta> objetivos)
        {
            foreach (var objetivoDto in objetivos)
            {
                var codigo = objetivoDto.Codigo.Replace("(", "").Replace(")", "");
                var anoString = codigo.Substring(3, 1);
                int.TryParse(anoString, out int ano);
                if (ano != 0)
                {
                    yield return new ObjetivoAprendizagemDto()
                    {
                        Descricao = objetivoDto.Descricao,
                        Id = objetivoDto.Id,
                        Ano = ano,
                        Codigo = codigo,
                        IdComponenteCurricular = objetivoDto.ComponenteCurricularId
                    };
                }
            }
        }

        private IEnumerable<ComponenteCurricular> ObterComponentesCurriculares()
        {
            var componentesCurriculares = repositorioComponenteCurricular.Listar();
            if (componentesCurriculares == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            return componentesCurriculares;
        }

        private IEnumerable<long> ObterComponentesJuremaPorIdEOL(IEnumerable<long> componentesCurricularesIds)
        {
            IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();

            var componentesFiltro = componentesCurriculares.Where(c => componentesCurricularesIds.Contains(c.CodigoEOL));
            var componentesJurema = componentesFiltro.Select(c => c.CodigoJurema);
            return componentesJurema;
        }
    }
}