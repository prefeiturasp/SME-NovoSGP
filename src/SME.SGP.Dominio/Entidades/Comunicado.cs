using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Comunicado : EntidadeBase
    {
        public Comunicado()
        {
            GruposComunicacao = new List<GrupoComunicacao>();
            Grupos = new List<ComunicadoGrupo>();
            Alunos = new List<ComunicadoAluno>();
            Turmas = new List<ComunicadoTurma>();
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IList<ComunicadoTurma> Turmas { get; set; }
        public bool AlunoEspecificado { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public Modalidade? Modalidade { get; set; }
        public int? Semestre { get; set; }
        public TipoComunicado TipoComunicado { get; set; }
        public IList<GrupoComunicacao> GruposComunicacao { get; set; }
        public List<ComunicadoGrupo> Grupos { get; set; }
        public IList<ComunicadoAluno> Alunos { get; set; }
        public string Titulo { get; set; }

        public void AdicionarGrupo(ComunicadoGrupo grupo)
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

        public void AdicionarTurma(ComunicadoTurma turma)
        {
            if (Id > 0)
                throw new NegocioException("Não é possivel editar as turmas de um comunicado");

            Turmas.Add(turma);
        }

        public void AdicionarTurma(string codigoTurma)
        {
            AdicionarTurma(new ComunicadoTurma
            {
                CodigoTurma = codigoTurma,
                ComunicadoId = Id
            });
        }

        public void SetarTipoComunicado()
        {
            TipoComunicado = IdentificarTipoComunicado();
        }

        private TipoComunicado IdentificarTipoComunicado()
        {
            if (AlunoEspecificado)
                return TipoComunicado.ALUNO;

            if (Turmas != null && Turmas.Any())
                return TipoComunicado.TURMA;

            if (Modalidade.HasValue)
                return TipoComunicado.UEMOD;

            if (!string.IsNullOrWhiteSpace(CodigoUe))
                return TipoComunicado.UE;

            if (!string.IsNullOrWhiteSpace(CodigoDre))
                return TipoComunicado.DRE;

            return TipoComunicado.SME;
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

        public void AtualizarIdTurmas()
        {
            if (Id == 0)
                return;

            Turmas = Turmas.Select(x =>
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