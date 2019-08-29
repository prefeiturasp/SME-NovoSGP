using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Auxiliares;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagem : IConsultasObjetivoAprendizagem
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IConfiguration configuration;
        private readonly IServicoJurema servicoJurema;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema, IRepositorioCache repositorioCache, IConfiguration configuration)
        {
            this.servicoJurema = servicoJurema ?? throw new System.ArgumentNullException(nameof(servicoJurema));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Listar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            var objetivos = new List<ObjetivoAprendizagemDto>();

            var objetivosCacheString = repositorioCache.Obter("ObjetivosAprendizagem");

            if (string.IsNullOrEmpty(objetivosCacheString))
            {
                var objetivosJuremaDto = await servicoJurema.ObterListaObjetivosAprendizagem();
                objetivos = MapearParaDto(objetivosJuremaDto).ToList();

                var tempoExpiracao = int.Parse(configuration.GetSection("ExpiracaoCache").GetSection("ObjetivosAprendizagem").Value);

                await repositorioCache.SalvarAsync("ObjetivosAprendizagem", Newtonsoft.Json.JsonConvert.SerializeObject(objetivos), tempoExpiracao);

            }
            else
                objetivos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObjetivoAprendizagemDto>>(objetivosCacheString);


            return objetivos.Where(c => (filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.Count > 0)?
                    filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.Contains(c.IdComponenteCurricular) : true
                    && (filtroObjetivosAprendizagemDto.Ano > 0) ? c.Ano == filtroObjetivosAprendizagemDto.Ano : true);
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
    }
}