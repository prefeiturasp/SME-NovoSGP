using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEventoMatricula : IConsultasEventoMatricula
    {
        private readonly IRepositorioEventoMatricula repositorio;

        public ConsultasEventoMatricula(IRepositorioEventoMatricula repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentException(nameof(repositorio));
        }
        public EventoMatriculaDto ObterUltimoEventoAluno(string codigoAluno, DateTime dataLimite)
        {
            return MapearParaDto(repositorio.ObterUltimoEventoAluno(codigoAluno, dataLimite));
        }

        private EventoMatriculaDto MapearParaDto(EventoMatricula eventoMatricula)
            => eventoMatricula == null ? null : new EventoMatriculaDto()
            { 
                CodigoAluno = eventoMatricula.CodigoAluno,
                DataEvento = eventoMatricula.DataEvento,
                Tipo = eventoMatricula.Tipo,
                NomeEscola = eventoMatricula.NomeEscola,
                NomeTurma = eventoMatricula.NomeTurma
            };
    }
}
