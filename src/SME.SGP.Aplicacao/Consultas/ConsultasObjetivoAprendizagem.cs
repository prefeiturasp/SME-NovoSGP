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
        private readonly Dictionary<int, string> Anos = new Dictionary<int, string>
        {
           {1,"first"},{2,"second"},{3,"third"},{4,"fourth"},{5,"fifth"},{6,"sixth"},{7,"seventh"},{8,"eighth"},{9,"nineth"}
        };

        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        private readonly IServicoJurema servicoJurema;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema,
                                                     IRepositorioCache repositorioCache,
                                                     IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                                     IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                     IConfiguration configuration,
                                                     IServicoUsuario servicoUsuario)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }

        public bool DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina)
        {
            IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();

            return componentesCurriculares.Any(x => x.CodigoEOL == codigoDisciplina);
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

            var objetivosCacheString = await repositorioCache.ObterAsync("ObjetivosAprendizagem");

            if (!string.IsNullOrWhiteSpace(objetivosCacheString))
                return JsonConvert.DeserializeObject<List<ObjetivoAprendizagemDto>>(objetivosCacheString);

            var objetivosJuremaDto = await servicoJurema.ObterListaObjetivosAprendizagem();

            objetivos = MapearParaDto(objetivosJuremaDto).ToList();

            var tempoExpiracao = int.Parse(configuration.GetSection("ExpiracaoCache").GetSection("ObjetivosAprendizagem").Value);

            await repositorioCache.SalvarAsync("ObjetivosAprendizagem", JsonConvert.SerializeObject(objetivos), tempoExpiracao);

            return objetivos;
        }

        public async Task<ObjetivoAprendizagemSimplificadoDto> ObterAprendizagemSimplificadaPorId(long id)
        {
            IEnumerable<ObjetivoAprendizagemDto> lstObjAprendizagemDtos = await Listar();

            ObjetivoAprendizagemDto objetivoDto = lstObjAprendizagemDtos.Where(obj => obj.Id == id).FirstOrDefault();

            return new ObjetivoAprendizagemSimplificadoDto()
            {
                Id = objetivoDto.Id,
                IdComponenteCurricular = objetivoDto.IdComponenteCurricular
            };
        }

        public async Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(int ano, int bimestre, long turmaId, long componenteCurricularId)
        {
            return repositorioObjetivosPlano.ObterDisciplinasDoBimestrePlanoAula(ano, bimestre, turmaId, componenteCurricularId);
        }

        public async Task<long> ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId)
        {
            return repositorioObjetivosPlano.ObterIdPorObjetivoAprendizagemJurema(planoId, objetivoAprendizagemJuremaId);
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var filtrarSomenteRegencia = regencia && !usuarioLogado.EhProfessorCj();
            var objetivosPlano = repositorioObjetivosPlano.ObterObjetivosPlanoDisciplina(ano,
                                                                                         bimestre,
                                                                                         turmaId,
                                                                                         componenteCurricularId,
                                                                                         disciplinaId,
                                                                                         filtrarSomenteRegencia);

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
                var ano = Anos.FirstOrDefault(x => x.Value.Equals(objetivoDto.Ano)).Key;
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

        private ObjetivoAprendizagemSimplificadoDto MapearParaDto(ObjetivoAprendizagemPlano objetivo)
        {
            return new ObjetivoAprendizagemSimplificadoDto()
            {
                Id = objetivo.Id,
                IdComponenteCurricular = objetivo.ComponenteCurricularId
            };
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