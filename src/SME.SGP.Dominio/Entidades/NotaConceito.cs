using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class NotaConceito : EntidadeBase
    {
        public string AlunoId { get; set; }
        public long AtividadeAvaliativaID { get; set; }
        public long? ConceitoId { get; set; }
        public string DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public TipoNota TipoNota { get; set; }

        public string ObterNota()
        {
            if (TipoNota == TipoNota.Conceito)
                return ConceitoId.ToString();
            else
            {
                if (!Nota.HasValue)
                    return Nota.ToString();

                return Nota.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public void Validar(string professorRf)
        {
            if (!CriadoRF.Equals(professorRf))
                throw new NegocioException("Não é possivel alterar a nota atribuida por outro professor");
        }

        public void ValidarConceitos(IEnumerable<Conceito> conceitos, string nomeAluno)
        {
            var conceito = conceitos.FirstOrDefault(c => c.Id == ConceitoId);

            if (conceito == null || (!conceito.Ativo && Id == 0))
                throw new NegocioException($"O conceito informado para o aluno {nomeAluno} não existe");
        }

        public void ValidarNota(NotaParametro notaParametro, string nomeAluno)
        {
            if (Nota < notaParametro.Minima)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} é menor que o minimo permitido");

            if (Nota > notaParametro.Maxima)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} é maior que o máximo permitido");

            var resto = Nota % notaParametro.Incremento;

            if (resto > 0)
                throw new NegocioException($"A nota informada para o aluno {nomeAluno} não possui um valor válido");
        }
    }
}