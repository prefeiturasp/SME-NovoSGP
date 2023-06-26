using SME.SGP.Dominio;
using System.Collections.Generic;
using System;

namespace SME.SGP.Infra
{
    public class AutenticacaoFrequenciaSGADto
    {
        public string Rf { get; set; }
        public string ComponenteCurricularCodigo { get; set; } 
        public Turma Turma { get; set; }
        public (UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>, bool, bool) usuarioAutenticado { get; set; }
    }
}
