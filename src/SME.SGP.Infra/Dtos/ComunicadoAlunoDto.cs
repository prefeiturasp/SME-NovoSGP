using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class ComunicadoAlunoDto
    {
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public long ComunicadoId { get; set; }

        public static explicit operator ComunicadoAlunoDto(ComunicadoAluno aluno)
            => aluno.EhNulo() ? null : new ComunicadoAlunoDto()
            {
                AlunoCodigo = aluno.AlunoCodigo,
                ComunicadoId = aluno.ComunicadoId,
                AlunoNome = aluno.AlunoNome
            };
    }
}
