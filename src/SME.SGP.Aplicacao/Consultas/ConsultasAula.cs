using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAula : IConsultasAula
    {
        private readonly IRepositorioAula repositorio;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasAula(IRepositorioAula repositorio,
                             IServicoUsuario servicoUsuario)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public AulaConsultaDto BuscarPorId(long id)
        {
            var aula = repositorio.ObterPorId(id);
            return MapearParaDto(aula);
        }

        public async Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(long calendarioId, string turma, string disciplina)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            return repositorio.ObterDatasDeAulasPorCalendarioTurmaEDisciplina(calendarioId, turma, disciplina, usuarioLogado.Id, usuarioLogado.PerfilAtual)?.Select(a => new DataAulasProfessorDto
            {
                Data = a.DataAula,
                IdAula = a.Id
            });
        }

        public async Task<int> ObterQuantidadeAulasTurmaSemana(string turma, string disciplina, string semana)
        {
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas;

            if (ExperienciaPedagogica(disciplina))
                aulas = await repositorio.ObterAulasTurmaExperienciasPedagogicasSemana(turma, semana);
            else
                aulas = await repositorio.ObterAulasTurmaDisciplinaSemana(turma, disciplina, semana);

            return aulas.Sum(a => a.Quantidade);
        }

        private bool ExperienciaPedagogica(string disciplina) 
            => new string[] { "1214", "1215", "1216", "1217", "1218", "1219", "1220", "1221", "1222", "1223" }
                .Contains(disciplina);

        private AulaConsultaDto MapearParaDto(Aula aula)
        {
            AulaConsultaDto dto = new AulaConsultaDto()
            {
                Id = aula.Id,
                DisciplinaId = aula.DisciplinaId,
                TurmaId = aula.TurmaId,
                UeId = aula.UeId,
                TipoCalendarioId = aula.TipoCalendarioId,
                TipoAula = aula.TipoAula,
                Quantidade = aula.Quantidade,
                DataAula = aula.DataAula,
                RecorrenciaAula = aula.RecorrenciaAula,
                AlteradoEm = aula.AlteradoEm,
                AlteradoPor = aula.AlteradoPor,
                AlteradoRF = aula.AlteradoRF,
                CriadoEm = aula.CriadoEm,
                CriadoPor = aula.CriadoPor,
                CriadoRF = aula.CriadoRF
            };
            return dto;
        }
    }
}