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

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula, 
                                IConsultasPlanoAnual consultasPlanoAnual,
                                IConsultasObjetivoAprendizagemAula consultasObjetivosAprendizagemAula)
        {
            this.repositorio = repositorioPlanoAula;
            this.consultasObjetivosAula = consultasObjetivosAprendizagemAula;
            this.consultasPlanoAnual = consultasPlanoAnual;
        }

        public async Task<PlanoAulaRetornoDto> ObterPlanoAulaPorTurmaDisciplina(DateTime data, long turmaId, string disciplinaId)
        {
            // Busca plano de aula por data e disciplina da aula
            var plano = await repositorio.ObterPlanoAulaPorDataDisciplina(data, turmaId.ToString(), disciplinaId);
            if (plano == null || plano.Id == 0)
                return null;

            var planoDto = MapearParaDto(plano) ?? new PlanoAulaRetornoDto();
            // Carrega objetivos já cadastrados no plano de aula
            var objetivosAula = await consultasObjetivosAula.ObterObjetivosPlanoAula(plano.Id);
            planoDto.ObjetivosAprendizagemAula = objetivosAula.Select(o => o.Id).ToList();

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
