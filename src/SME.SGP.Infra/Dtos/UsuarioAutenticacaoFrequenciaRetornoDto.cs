using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;

namespace SME.SGP.Infra
{
    public class UsuarioAutenticacaoFrequenciaRetornoDto
    {

        public UsuarioAutenticacaoFrequenciaRetornoDto(UsuarioAutenticacaoRetornoDto usuarioAutenticacao, TurmaUeDreDto turma, string componenteCurricularCodigo)
        {
            UsuarioAutenticacao = usuarioAutenticacao;
            Turma = turma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }
        public UsuarioAutenticacaoRetornoDto UsuarioAutenticacao { get; set; }
        public TurmaUeDreDto Turma { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }
}