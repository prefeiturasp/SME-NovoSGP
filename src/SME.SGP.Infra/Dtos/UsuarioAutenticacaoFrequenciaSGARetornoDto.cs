using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class UsuarioAutenticacaoFrequenciaSGARetornoDto
    {

        public UsuarioAutenticacaoFrequenciaSGARetornoDto(UsuarioAutenticacaoRetornoDto usuarioAutenticacao, Turma turma, string componenteCurricularCodigo)
        {
            UsuarioAutenticacao = usuarioAutenticacao;
            Turma = turma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }
        public UsuarioAutenticacaoRetornoDto UsuarioAutenticacao { get; set; }
        public Turma Turma { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }
}