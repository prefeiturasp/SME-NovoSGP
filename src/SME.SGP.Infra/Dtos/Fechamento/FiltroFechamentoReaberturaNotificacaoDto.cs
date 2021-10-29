using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroFechamentoReaberturaNotificacaoDto
    {
        public FiltroFechamentoReaberturaNotificacaoDto(FechamentoReabertura fechamentoReabertura, Usuario usuario)
        {
            FechamentoReabertura = fechamentoReabertura;
            Usuario = usuario;
        }

        public FechamentoReabertura FechamentoReabertura { get; set; }
        public Usuario Usuario { get; set; }
    }
}