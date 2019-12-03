
using SME.SGP.Dominio.Entidades;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class NotasConceitos : EntidadeBase
    {
        public long AtividadeAvaliativaID { get; set; }
        public string AlunoId { get; set; }
        public int Nota { get; set; }
        public long Conceito { get; set; }
        public long TipoNota { get; set; }
        
        public void Validar(string alteradorRf)
        {
            if (!CriadoRF.Equals(alteradorRf))
                throw new NegocioException("Não e possivel alterar a nota atribuida por outro professor");
        }

        public void ValidarNota(NotaParametro notaParametro, string nomeAluno)
        {
            if (Nota < notaParametro.Minima)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} é menor que o minimo permitido");

            if (Nota > notaParametro.Maxima)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} é maior que o maximo permitido");

            var resto = Nota % notaParametro.Incremento;

            if (resto > 0)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} não possui um valor valido");
        }

        public void ValidarConceitos(IEnumerable<Conceito> conceitos, string nomeAluno)
        {
            var conceito = conceitos.FirstOrDefault(c => c.Id == Conceito);

            if (conceito == null || (!conceito.Ativo && Id == 0))
                throw new NegocioException($"O conceito informado para o aluno {nomeAluno} não existe");            
        }

    }
}
