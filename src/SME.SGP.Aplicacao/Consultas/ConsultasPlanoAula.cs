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

        public async Task<PlanoAulaRetornoDto> ObterPlanoAulaPorAula(long aulaId)
        {
            PlanoAulaRetornoDto planoAulaDto = new PlanoAulaRetornoDto();
            // Busca plano de aula por data e disciplina da aula
            var plano = await repositorio.ObterPlanoAulaPorAula(aulaId);
            var aulaDto = consultasAula.BuscarPorId(aulaId);

            if (plano != null)
            {
                planoAulaDto = MapearParaDto(plano) ?? new PlanoAulaRetornoDto();

                // Carrega objetivos aprendizagem Jurema
                var planoAnual = await consultasPlanoAnual.ObterPorEscolaTurmaAnoEBimestre(new FiltroPlanoAnualDto()
                {
                    AnoLetivo = aulaDto.DataAula.Year,
                    Bimestre = (aulaDto.DataAula.Month + 2) / 3,
                    ComponenteCurricularEolId = long.Parse(aulaDto.DisciplinaId),
                    EscolaId = aulaDto.UeId,
                    TurmaId = int.Parse(aulaDto.TurmaId)
                });

                // Carrega objetivos já cadastrados no plano de aula
                var objetivosAula = await consultasObjetivosAula.ObterObjetivosPlanoAula(plano.Id);
                // Filtra objetivos anual com os objetivos da aula
                planoAulaDto.ObjetivosAprendizagemAula = planoAnual.ObjetivosAprendizagem
                                    .Where(c => objetivosAula.Any(a => a.ObjetivoAprendizagemPlano.ObjetivoAprendizagemJuremaId == c.Id))
                                    .ToList();
            }

            // Carrega informações da aula para o retorno
            planoAulaDto.AulaId = aulaDto.Id;
            planoAulaDto.QtdAulas = aulaDto.Quantidade;

            return planoAulaDto;
        }

        public bool ValidarPlanoAulaExistente(FiltroPlanoAulaDto filtroPlanoAulaDto)
        {
            return repositorio.ValidarPlanoExistentePorTurmaDataEDisciplina(filtroPlanoAulaDto.Data, filtroPlanoAulaDto.TurmaId.ToString(), filtroPlanoAulaDto.DisciplinaId  );
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAula plano) =>
            plano == null ? null :
            new PlanoAulaRetornoDto()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
                DesenvolvimentoAula = plano.DesenvolvimentoAula,
                RecuperacaoAula = plano.RecuperacaoAula,
                LicaoCasa = plano.LicaoCasa,
                AulaId = plano.AulaId
            };
    }
}
