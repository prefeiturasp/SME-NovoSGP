using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPlanoAula : IConsultasPlanoAula
    {
        private readonly IConsultasAula consultasAula;
        private readonly IConsultasObjetivoAprendizagemAula consultasObjetivosAula;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                                IConsultasPlanoAnual consultasPlanoAnual,
                                IConsultasObjetivoAprendizagemAula consultasObjetivosAprendizagemAula,
                                IConsultasAula consultasAula,
                                IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                IServicoUsuario servicoUsuario,
                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorio = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.consultasObjetivosAula = consultasObjetivosAprendizagemAula ?? throw new ArgumentNullException(nameof(consultasObjetivosAprendizagemAula));
            this.consultasPlanoAnual = consultasPlanoAnual ?? throw new ArgumentNullException(nameof(consultasPlanoAnual));
            this.consultasAula = consultasAula ?? throw new ArgumentNullException(nameof(consultasAula));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<bool> PlanoAulaRegistrado(long aulaId)
            => await repositorio.PlanoAulaRegistradoAsync(aulaId);

        public IEnumerable<PlanoAulaExistenteRetornoDto> ValidarPlanoAulaExistente(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto)
        {
            IList<PlanoAulaExistenteRetornoDto> retorno = new List<PlanoAulaExistenteRetornoDto>();
            var planoAulaTurmaDatasDto = filtroPlanoAulaExistenteDto.PlanoAulaTurmaDatas;

            for (int i = 0; i < planoAulaTurmaDatasDto.Count; i++)
            {
                retorno.Add(new PlanoAulaExistenteRetornoDto()
                {
                    TurmaId = filtroPlanoAulaExistenteDto.PlanoAulaTurmaDatas[i].TurmaId,
                    Existe = repositorio.ValidarPlanoExistentePorTurmaDataEDisciplina(
                                    planoAulaTurmaDatasDto[i].Data,
                                    planoAulaTurmaDatasDto[i].TurmaId.ToString(),
                                    planoAulaTurmaDatasDto[i].DisciplinaId)
                });
            }

            return retorno;
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
                AulaId = plano.AulaId,

                Migrado = plano.Migrado,
                CriadoEm = plano.CriadoEm,
                CriadoPor = plano.CriadoPor,
                CriadoRf = plano.CriadoRF,
                AlteradoEm = plano.AlteradoEm,
                AlteradoPor = plano.AlteradoPor,
                AlteradoRf = plano.AlteradoRF
            };
    }
}