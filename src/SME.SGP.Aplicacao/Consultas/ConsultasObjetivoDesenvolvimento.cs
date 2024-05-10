using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoDesenvolvimento : IConsultasObjetivoDesenvolvimento
    {
        private readonly IRepositorioObjetivoDesenvolvimento repositorioObjetivoDesenvolvimento;

        public ConsultasObjetivoDesenvolvimento(IRepositorioObjetivoDesenvolvimento repositorioObjetivoDesenvolvimento)
        {
            this.repositorioObjetivoDesenvolvimento = repositorioObjetivoDesenvolvimento ?? throw new System.ArgumentNullException(nameof(repositorioObjetivoDesenvolvimento));
        }

        public IEnumerable<ObjetivoDesenvolvimentoDto> Listar()
        {
            return MapearParaDto(repositorioObjetivoDesenvolvimento.Listar());
        }

        private IEnumerable<ObjetivoDesenvolvimentoDto> MapearParaDto(IEnumerable<RecuperacaoParalelaObjetivoDesenvolvimento> objetivos)
        {
            return objetivos?.Select(m => new ObjetivoDesenvolvimentoDto()
            {
                Descricao = m.Descricao,
                Id = m.Id
            });
        }
    }
}