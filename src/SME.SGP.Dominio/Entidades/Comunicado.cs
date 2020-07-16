using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Comunicado : EntidadeBase
    {
        public Comunicado()
        {
            Grupos = new List<GrupoComunicacao>();
            Alunos = new List<ComunicadoAluno>();
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Turma { get; set; }
        public bool AlunoEspecificado { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public Modalidade? Modalidade { get; set; }
        public List<GrupoComunicacao> Grupos { get; set; }
        public IList<ComunicadoAluno> Alunos { get; internal set; }
        public string Titulo { get; set; }

        public void AdicionarGrupo(GrupoComunicacao grupo)
        {
            Grupos.Add(grupo);
        }

        public void AdicionarAluno(ComunicadoAluno aluno)
        {
            if (Id > 0)
                throw new NegocioException("Não é possivel editar os alunos de um comunicado");

            Alunos.Add(aluno);
        }

        public void AdicionarAluno(string codigoAluno)
        {
            AdicionarAluno(new ComunicadoAluno
            {
                AlunoCodigo = codigoAluno,
                ComunicadoId = Id
            });
        }

        public void RemoverAluno(string codigoAluno)
        {
            if (Id > 0)
                throw new NegocioException("Não é possivel editar os alunos de um comunicado");
        }

        public void AtualizarIdAlunos()
        {
            if (Id == 0)
                return;

            Alunos = Alunos.Select(x =>
            {
                x.ComunicadoId = Id;

                return x;
            }).ToList();
        }

        public void MarcarExcluido()
        {
            Excluido = true;
        }
    }
}