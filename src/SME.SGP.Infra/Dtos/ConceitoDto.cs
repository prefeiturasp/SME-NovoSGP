using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConceitoDto
    {
        public long Id { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public bool Aprovado { get; set; }

        public static explicit operator ConceitoDto(Conceito conceito)
            => new ConceitoDto
            {
                Aprovado = conceito.Aprovado,
                Descricao = conceito.Descricao,
                Id = conceito.Id,
                Valor = conceito.Valor
            };
    }
}
