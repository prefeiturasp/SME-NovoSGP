using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaPeriodoPAPDto
    {
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public long Id { get; set; }
        public AuditoriaDto Auditoria { get; set; }

        public static explicit operator RecuperacaoParalelaPeriodoPAPDto(RecuperacaoParalelaPeriodo recuperacaoParalelaPeriodo) => new RecuperacaoParalelaPeriodoPAPDto
        {
            Auditoria = (AuditoriaDto)recuperacaoParalelaPeriodo,
            Descricao = recuperacaoParalelaPeriodo.Descricao,
            Excluido = recuperacaoParalelaPeriodo.Excluido,
            Nome = recuperacaoParalelaPeriodo.Nome,
            Id = recuperacaoParalelaPeriodo.Id
        };
    }
}
