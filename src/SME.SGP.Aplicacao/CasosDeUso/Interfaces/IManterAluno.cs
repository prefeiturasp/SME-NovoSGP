using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IManterAluno
    {
        void Salvar(AlunoDto alunoDto);
        void SalvarCriandoProfessor(AlunoDto alunoDto);
        IEnumerable<AlunoDto> Listar(int pagina, int tamanho);
    }
}