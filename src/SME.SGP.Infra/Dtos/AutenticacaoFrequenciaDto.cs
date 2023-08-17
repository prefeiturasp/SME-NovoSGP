using SME.SGP.Dominio;
using System.Collections.Generic;
using System;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Infra
{
    public class AutenticacaoFrequenciaDto
    {
        public string Rf { get; set; }
        public string ComponenteCurricularCodigo { get; set; } 
        public TurmaUeDreDto Turma { get; set; }
        public (UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ) UsuarioAutenticacao { get; set; }
    }
}
