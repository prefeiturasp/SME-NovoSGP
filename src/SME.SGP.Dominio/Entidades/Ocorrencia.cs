using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SME.SGP.Dominio
{
    public class Ocorrencia : EntidadeBase
    {
        public ICollection<OcorrenciaAluno> Alunos { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public TimeSpan? HoraOcorrencia { get; set; }
        public OcorrenciaTipo OcorrenciaTipo { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public string Titulo { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }

        public Ocorrencia(DateTime dataOcorrencia, string titulo, string descricao, OcorrenciaTipo ocorrenciaTipo, Turma turma)
        {
            DataOcorrencia = dataOcorrencia;
            Titulo = titulo;
            Descricao = descricao;
            SetOcorrenciaTipo(ocorrenciaTipo);
            SetTurma(turma);
        }

        public Ocorrencia(DateTime dataOcorrencia, string horaOcorrencia, string titulo, string descricao, OcorrenciaTipo ocorrenciaTipo, Turma turma)
            : this(dataOcorrencia, titulo, descricao, ocorrenciaTipo, turma)
        {
            SetHoraOcorrencia(horaOcorrencia);
        }

        protected Ocorrencia()
        {
        }

        public void AdicionarAluno(long codigoAluno)
        {
            var ocorrenciaAluno = new OcorrenciaAluno(codigoAluno, this);
            Alunos = Alunos ?? new List<OcorrenciaAluno>();
            Alunos.Add(ocorrenciaAluno);
        }

        public void AdiconarAlunos(IEnumerable<long> codigosAlunos)
        {
            foreach (var codigoAluno in codigosAlunos)
                AdicionarAluno(codigoAluno);
        }

        public void SetHoraOcorrencia(string horaOcorrencia)
        {
            if (string.IsNullOrWhiteSpace(horaOcorrencia))
            {
                HoraOcorrencia = null;
                return;
            }

            if (!Regex.IsMatch(horaOcorrencia, "^([01][0-9]|2[0-3]):([0-5][0-9])$"))
                throw new NegocioException("A hora informada está em um formato inválido.");

            var horaOcorrenciaSplit = horaOcorrencia.Split(':');
            var hora = int.Parse(horaOcorrenciaSplit[0]);
            var minutos = int.Parse(horaOcorrenciaSplit[1]);
            HoraOcorrencia = new TimeSpan(hora, minutos, 0);
        }

        public void SetOcorrenciaTipo(OcorrenciaTipo ocorrenciaTipo)
        {
            if (ocorrenciaTipo is null)
                throw new NegocioException("É necessário informar o tipo de ocorrência.");

            OcorrenciaTipo = ocorrenciaTipo;
            OcorrenciaTipoId = ocorrenciaTipo.Id;
        }

        public void SetTurma(Turma turma)
        {
            if(turma is null)
                throw new NegocioException("É necessário informar o tipo de ocorrência.");

            Turma = turma;
            TurmaId = turma.Id;
        }

        public void Excluir() => Excluido = true;
    }
}