using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagem : IConsultasObjetivoAprendizagem
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoJurema servicoJurema;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema, IRepositorioCache repositorioCache)
        {
            this.servicoJurema = servicoJurema ?? throw new System.ArgumentNullException(nameof(servicoJurema));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Listar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            var objetivos = new List<ObjetivoAprendizagemDto>();

            var objetivosCacheString = repositorioCache.Obter("ObjetivosAprendizagem");

            if (string.IsNullOrEmpty(objetivosCacheString))
            {
                var objetivosJuremaDto = await servicoJurema.ObterListaObjetivosAprendizagem();
                objetivos = MapearParaDto(objetivosJuremaDto);

                await repositorioCache.SalvarAsync("ObjetivosAprendizagem", Newtonsoft.Json.JsonConvert.SerializeObject(objetivos));
            }
            else
            {
                objetivos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObjetivoAprendizagemDto>>(objetivosCacheString);
            }

            //return objetivos.Where(c =>
            //        filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.Contains(c.IdComponenteCurricular)
            //        && c.Ano == filtroObjetivosAprendizagemDto.Ano)
            //    .ToList();

            return objetivos
                .ToList();
        }

        private List<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagemResposta> objetivos)
        {
            return objetivos?.Select(m => new ObjetivoAprendizagemDto()
            {
                Descricao = m.Descricao,
                Id = m.Id,
                Ano = m.Codigo.Substring(3, 2),
                AtualizadoEm = m.AtualizadoEm,
                Codigo = m.Codigo,
                CriadoEm = m.CriadoEm,
                IdComponenteCurricular = m.ComponenteCurricularId
            }).ToList();
        }
    }
}