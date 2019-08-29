using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagem : IConsultasObjetivoAprendizagem
    {
        private readonly IServicoJurema servicoJurema;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
        }

        public IEnumerable<ObjetivoAprendizagemDto> Listar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            var objetivos = servicoJurema.ObterListaObjetivosAprendizagem();

            return MapearParaDto(objetivos.Where(c =>
                    filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.Contains(c.ComponenteCurricularId) &&
                    c.Codigo.Substring(3, 2).Equals(filtroObjetivosAprendizagemDto.Ano)));
        }

        private IEnumerable<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagemResposta> objetivos)
        {
            return objetivos?.Select(m => new ObjetivoAprendizagemDto()
            {
                Descricao = m.Descricao,
                Id = m.Id,
                Ano = m.Ano,
                AtualizadoEm = m.AtualizadoEm,
                Codigo = m.Codigo,
                CriadoEm = m.CriadoEm,
                IdComponenteCurricular = m.ComponenteCurricularId
            });
        }
    }
}