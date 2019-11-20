using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPlanoAula : IConsultasPlanoAula
    {
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivosAnual;
        private readonly IConsultasObjetivoAprendizagemAula consultasObjetivosAula;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula, 
                                IConsultasPlanoAnual consultasPlanoAnual,
                                IConsultasObjetivoAprendizagem consultasObjetivosAprendizagemAnual,
                                IConsultasObjetivoAprendizagemAula consultasObjetivosAprendizagemAula)
        {
            this.repositorio = repositorioPlanoAula;
            this.consultasObjetivosAnual = consultasObjetivosAprendizagemAnual;
            this.consultasObjetivosAula = consultasObjetivosAprendizagemAula;
            this.consultasPlanoAnual = consultasPlanoAnual;
        }

        public async Task<PlanoAulaRetornoDto> ObterPlanoAulaPorTurmaDisciplina(
            DateTime data, 
            string escolaId, 
            int turmaId, 
            string disciplinaId)
        {
            // Busca plano de aula por data e disciplina da aula
            var plano = await repositorio.ObterPlanoAulaPorDataDisciplina(data, disciplinaId);
            var planoDto = MapearParaDto(plano) ?? new PlanoAulaRetornoDto();

            // Busca objetivos de aprendizagem do plano anual para o bimestre
            var filtro = new FiltroPlanoAnualDto()
            {
                AnoLetivo = data.Year,
                Bimestre = Bimestre(data.Month),
                ComponenteCurricularEolId = long.Parse(disciplinaId),
                EscolaId = escolaId,
                TurmaId = turmaId
            };
            var planoAnual = await consultasPlanoAnual.ObterPorEscolaTurmaAnoEBimestre(filtro);
            if (planoAnual == null)
                throw new NegocioException("Plano anual não localizado para turma e disciplina.");

            // carrega objetivos do plano anual
            planoDto.ObjetivosAprendizagem = planoAnual.ObjetivosAprendizagem;

            // Carrega objetivos já cadastrados no plano de aula
            if (plano != null && plano.Id > 0)
            {
                var objetivosAula = await consultasObjetivosAula.ObterObjetivosPlanoAula(plano.Id);
                planoDto.ObjetivosAprendizagemAula = objetivosAula.Select(o => o.Id).ToList();
            }

            return planoDto;
        }
        private int Bimestre(int mes)
        {
            var lista = new List<int>()
            {
                1, 1, 1,
                2, 2, 2,
                3, 3, 3,
                4, 4, 4
            };
            return lista[mes];
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAula plano) =>
            plano == null ? null :
            new PlanoAulaRetornoDto()
            {
                Descricao = plano.Descricao,
                DesenvolvimentoAula = plano.DesenvolvimentoAula,
                RecuperacaoAula = plano.RecuperacaoAula,
                LicaoCasa = plano.LicaoCasa,
                AulaId = plano.AulaId
            };

    }
}
