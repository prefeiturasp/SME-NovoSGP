﻿using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioEncaminhamentoNaapaDetalhadoDto
    {
        public long[] EncaminhamentoNaapaIds { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public EnumImprimirAnexosNAAPA ImprimirAnexos { get; set; }
    }
}