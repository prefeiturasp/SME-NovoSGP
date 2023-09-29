using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SME.SGP.Dominio
{
    public class Ocorrencia : EntidadeBase
    {
        public ICollection<OcorrenciaAluno> Alunos { get; set; }
        public ICollection<OcorrenciaServidor> Servidores { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public TimeSpan? HoraOcorrencia { get; set; }
        public OcorrenciaTipo OcorrenciaTipo { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public string Titulo { get; set; }
        public long? TurmaId { get; set; }
        public Turma Turma { get; set; }
        public Ue Ue { get; set; }
        public long UeId { get; set; }

        public Ocorrencia(DateTime dataOcorrencia, string titulo, string descricao, OcorrenciaTipo ocorrenciaTipo, long? turmaId, long ueId) : this()
        {
            DataOcorrencia = dataOcorrencia;
            Titulo = titulo;
            Descricao = descricao;
            SetOcorrenciaTipo(ocorrenciaTipo);
            TurmaId = turmaId;
            UeId = ueId;
        }

        public Ocorrencia(DateTime dataOcorrencia, string horaOcorrencia, string titulo, string descricao, OcorrenciaTipo ocorrenciaTipo, long? turmaId, long ueId)
            : this(dataOcorrencia, titulo, descricao, ocorrenciaTipo, turmaId, ueId)
        {
            Alunos = Alunos ?? new List<OcorrenciaAluno>();
            Servidores = Servidores ?? new List<OcorrenciaServidor>();
            SetHoraOcorrencia(horaOcorrencia);
        }

        public Ocorrencia()
        {
            Alunos = new List<OcorrenciaAluno>();
            Servidores = new List<OcorrenciaServidor>();
        }

        public void AdicionarAluno(long codigoAluno)
        {
            var ocorrenciaAluno = new OcorrenciaAluno(codigoAluno, this);
            Alunos.Add(ocorrenciaAluno);
        }

        public void AdicinarServidor(string codigoServidor)
        {
            var ocorrenciaServidor = new OcorrenciaServidor(codigoServidor, this);
            Servidores.Add(ocorrenciaServidor);
        }
        public void AdiconarAlunos(IEnumerable<long> codigosAlunos)
        {
            foreach (var codigoAluno in codigosAlunos)
                AdicionarAluno(codigoAluno);
        }

        public void AdicionarServidores(IEnumerable<string> codigosServidor)
        {
            foreach (var codigoServidor in codigosServidor)
                AdicinarServidor(codigoServidor);
        }

        public void SetHoraOcorrencia(string horaOcorrencia)
        {
            if (string.IsNullOrWhiteSpace(horaOcorrencia))
            {
                HoraOcorrencia = null;
                return;
            }

            var horaOcorrenciaSplit = horaOcorrencia.Split(':');
            try
            {
                var hora = int.Parse(horaOcorrenciaSplit[0]);
                var minutos = int.Parse(horaOcorrenciaSplit[1]);
                HoraOcorrencia = new TimeSpan(hora, minutos, 0);
            }
            catch (Exception)
            {
                HoraOcorrencia = null;
            }
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
            if (turma.NaoEhNulo())
            {
                Turma = turma;
                TurmaId = turma.Id;    
            }
        }

        public void Excluir() => Excluido = true;
    }
}