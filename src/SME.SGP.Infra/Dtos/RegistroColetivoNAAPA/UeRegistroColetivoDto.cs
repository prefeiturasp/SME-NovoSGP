using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class UeRegistroColetivoDto
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string NomeFormatado => TipoEscola != TipoEscola.Nenhum ? $"{TipoEscola.ObterNomeCurto()} {Nome}" : $"{Nome}";
    }
}
