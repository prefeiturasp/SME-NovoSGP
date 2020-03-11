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
        private readonly IRepositorioAula repositorioAula;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                                IConsultasPlanoAnual consultasPlanoAnual,
                                IConsultasObjetivoAprendizagemAula consultasObjetivosAprendizagemAula,
                                IConsultasAula consultasAula,
                                IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                IRepositorioAula repositorioAula,
                                IServicoUsuario servicoUsuario)
        {
            this.repositorio = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.consultasObjetivosAula = consultasObjetivosAprendizagemAula ?? throw new ArgumentNullException(nameof(consultasObjetivosAprendizagemAula));
            this.consultasPlanoAnual = consultasPlanoAnual ?? throw new ArgumentNullException(nameof(consultasPlanoAnual));
            this.consultasAula = consultasAula ?? throw new ArgumentNullException(nameof(consultasAula));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<PlanoAulaRetornoDto> ObterPlanoAulaPorAula(long aulaId)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            if (!await VerificarPlanoAnualExistente(aulaId))
            {
                throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");
            }
            PlanoAulaRetornoDto planoAulaDto = new PlanoAulaRetornoDto();
            // Busca plano de aula por data e disciplina da aula
            var plano = await repositorio.ObterPlanoAulaPorAula(aulaId);
            var aulaDto = await consultasAula.BuscarPorId(aulaId);
            var atividadeAvaliativa = await repositorioAtividadeAvaliativa.ObterAtividadeAvaliativa(aulaDto.DataAula.Date, aulaDto.DisciplinaId, aulaDto.TurmaId, aulaDto.UeId);
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
                    TurmaId = aulaDto.TurmaId
                }, seNaoExistirRetornaNovo: false);

                // Carrega objetivos já cadastrados no plano de aula
                var objetivosAula = await consultasObjetivosAula.ObterObjetivosPlanoAula(plano.Id);

                if (planoAnual != null)
                {
                    // Filtra objetivos anual com os objetivos da aula
                    planoAulaDto.ObjetivosAprendizagemAula = planoAnual.ObjetivosAprendizagem
                                        .Where(c => objetivosAula.Any(a => a.ObjetivoAprendizagemPlano.ObjetivoAprendizagemJuremaId == c.Id))
                                        .ToList();
                }
            }
            var periodoEscolar = consultasPeriodoEscolar.ObterPorTipoCalendario(aulaDto.TipoCalendarioId);
            var periodo = periodoEscolar.Periodos.FirstOrDefault(p => p.PeriodoInicio <= aulaDto.DataAula && p.PeriodoFim >= aulaDto.DataAula);
            var planoAnualId = await consultasPlanoAnual.ObterIdPlanoAnualPorAnoEscolaBimestreETurma(
                        aulaDto.DataAula.Year, aulaDto.UeId, long.Parse(aulaDto.TurmaId), periodo.Bimestre, long.Parse(aulaDto.DisciplinaId));

            // Carrega informações da aula para o retorno
            planoAulaDto.PossuiPlanoAnual = planoAnualId > 0;
            planoAulaDto.AulaId = aulaDto.Id;
            planoAulaDto.QtdAulas = aulaDto.Quantidade;
            planoAulaDto.IdAtividadeAvaliativa = atividadeAvaliativa?.Id;
            planoAulaDto.PodeLancarNota = planoAulaDto.IdAtividadeAvaliativa.HasValue && aulaDto.DataAula.Date <= DateTime.Now.Date;
            return planoAulaDto;
        }

        public async Task<bool> PlanoAulaRegistrado(long aulaId)
            => await repositorio.PlanoAulaRegistrado(aulaId);

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

        private async Task<bool> VerificarPlanoAnualExistente(long aulaId)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = repositorioAula.ObterPorId(aulaId);
            var periodoEscolar = consultasPeriodoEscolar.ObterPeriodoEscolarPorData(aula.TipoCalendarioId, aula.DataAula);
            var planoAnualId = await consultasPlanoAnual.ObterIdPlanoAnualPorAnoEscolaBimestreETurma(
                        aula.DataAula.Year, aula.UeId, long.Parse(aula.TurmaId), periodoEscolar.Bimestre, long.Parse(aula.DisciplinaId));
            if (planoAnualId <= 0 && !usuario.EhProfessorCj())
                return false;
            return true;
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