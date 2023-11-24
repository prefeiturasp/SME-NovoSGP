using MediatR;
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
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                                IServicoUsuario servicoUsuario,
                                IMediator mediator)
        {
            this.repositorio = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> PlanoAulaRegistrado(long aulaId)
            => await repositorio.PlanoAulaRegistradoAsync(aulaId);

        public async Task<IEnumerable<PlanoAulaExistenteRetornoDto>> ValidarPlanoAulaExistente(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto)
        {
            IList<PlanoAulaExistenteRetornoDto> retorno = new List<PlanoAulaExistenteRetornoDto>();
            var planoAulaTurmaDatasDto = filtroPlanoAulaExistenteDto.PlanoAulaTurmaDatas;

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            for (int i = 0; i < planoAulaTurmaDatasDto.Count; i++)
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(planoAulaTurmaDatasDto[i].TurmaId.ToString(), usuarioLogado.Login, usuarioLogado.PerfilAtual, true));

                if (componentesCurriculares.EhNulo())
                    componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(planoAulaTurmaDatasDto[i].TurmaId.ToString(), usuarioLogado.Login, usuarioLogado.PerfilAtual, true, false));

                string disciplina = componentesCurriculares
                                .Where(c => c.TerritorioSaber && c.Codigo.ToString() == planoAulaTurmaDatasDto[i].DisciplinaId)
                                .Select(c => (long?)c.CodigoComponenteTerritorioSaber ?? c.Codigo)
                                .FirstOrDefault().ToString();

                retorno.Add(new PlanoAulaExistenteRetornoDto()
                {
                    TurmaId = filtroPlanoAulaExistenteDto.PlanoAulaTurmaDatas[i].TurmaId,
                    Existe = repositorio.ValidarPlanoExistentePorTurmaDataEDisciplina(
                                    planoAulaTurmaDatasDto[i].Data,
                                    planoAulaTurmaDatasDto[i].TurmaId.ToString(),
                                    disciplina)
                });
            }

            return retorno;
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAula plano) =>
            plano.EhNulo() ? null :
            new PlanoAulaRetornoDto()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
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