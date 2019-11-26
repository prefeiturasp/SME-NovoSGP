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
        private readonly IConsultasObjetivoAprendizagemAula consultasObjetivosAula;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IConsultasAula consultasAula;

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula, 
                                IConsultasPlanoAnual consultasPlanoAnual,
                                IConsultasObjetivoAprendizagemAula consultasObjetivosAprendizagemAula,
                                IConsultasAula consultasAula)
        {
            this.repositorio = repositorioPlanoAula;
            this.consultasObjetivosAula = consultasObjetivosAprendizagemAula;
            this.consultasPlanoAnual = consultasPlanoAnual;
            this.consultasAula = consultasAula;
        }

        public async Task<PlanoAulaRetornoDto> ObterPlanoAulaPorTurmaDisciplina(DateTime data, long turmaId, string disciplinaId)
        {
            PlanoAulaRetornoDto planoAulaDto = new PlanoAulaRetornoDto();
            // Busca plano de aula por data e disciplina da aula
            var plano = await repositorio.ObterPlanoAulaPorDataDisciplina(data, turmaId.ToString(), disciplinaId);

            if (plano != null)
            {
                planoAulaDto = MapearParaDto(plano) ?? new PlanoAulaRetornoDto();
                // Carrega objetivos já cadastrados no plano de aula
                var objetivosAula = await consultasObjetivosAula.ObterObjetivosPlanoAula(plano.Id);
                planoAulaDto.ObjetivosAprendizagemAula = objetivosAula.Select(o => o.Id).ToList();
            }

            // Carrega informações da aula para o retorno
            var aulaDto = await consultasAula.ObterAulaDataTurmaDisciplina(data, turmaId.ToString(), disciplinaId);
            planoAulaDto.AulaId = aulaDto.Id;
            planoAulaDto.QtdAulas = aulaDto.Quantidade;

            return planoAulaDto;
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
