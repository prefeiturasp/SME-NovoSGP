﻿namespace SME.SGP.Infra
{
    public class AtribuicaoCJListaFiltroDto
    {
        public string UeId { get; set; }
        public int AnoLetivo { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public bool Historico { get; set; }
    }
}