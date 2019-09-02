using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultaDres : IConsultaDres
    {
        private readonly IServicoEOL servicoEOL;

        public ConsultaDres(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public IEnumerable<DreConsultaDto> ObterTodos()
        {
            var respostaEol = servicoEOL.ObterDres();

            return MapearParaDto(respostaEol);
        }

        private IEnumerable<DreConsultaDto> MapearParaDto(IEnumerable<DreRespostaEolDto> respostaEol)
        {
            foreach (var item in respostaEol)
            {
                yield return new DreConsultaDto()
                {
                    Id = item.CodigoDRE,
                    Nome = item.NomeDRE,
                    Sigla = item.SiglaDRE
                };
            }
        }
    }
}