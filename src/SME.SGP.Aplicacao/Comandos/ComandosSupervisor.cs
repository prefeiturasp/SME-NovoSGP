using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosSupervisor : IComandosSupervisor
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ComandosSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                  IUnitOfWork unitOfWork,
                                  IServicoEol servicoEol,
                                  IMediator mediator)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task AtribuirUE(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto)
        {
            await ValidarDados(atribuicaoSupervisorEscolaDto);

            var escolasAtribuidas = repositorioSupervisorEscolaDre
                .ObtemPorDreESupervisor(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.SupervisorId, true);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                await AjustarRegistrosExistentes(atribuicaoSupervisorEscolaDto, escolasAtribuidas);
                AtribuirEscolas(atribuicaoSupervisorEscolaDto);

                unitOfWork.PersistirTransacao();
            }
        }

        private async Task ValidarDados(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto)
        {
            var dre = await mediator
                .Send(new ObterDREIdPorCodigoQuery(atribuicaoSupervisorEscolaDto.DreId));

            if (dre < 1)
                throw new NegocioException($"A DRE {atribuicaoSupervisorEscolaDto.DreId} não foi localizada.");

            var supervisores = servicoEol
                .ObterSupervisoresPorDre(atribuicaoSupervisorEscolaDto.DreId);

            atribuicaoSupervisorEscolaDto.UESIds
                .ForEach(ue =>
                {
                    var ueLocalizada = mediator.Send(new ObterUeComDrePorCodigoQuery(ue)).Result;

                    if (ueLocalizada == null)
                        throw new NegocioException($"A UE {ue} não foi localizada.");

                    if (!ueLocalizada.Dre.CodigoDre.Equals(atribuicaoSupervisorEscolaDto.DreId))
                        throw new NegocioException($"A UE {ue} não pertence a DRE {atribuicaoSupervisorEscolaDto.DreId}.");
                });

            if (!supervisores.Any(s => s.CodigoRF.Equals(atribuicaoSupervisorEscolaDto.SupervisorId)))
            {                
                var atribuicaoExistentes = repositorioSupervisorEscolaDre
                    .ObtemPorDreESupervisor(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.SupervisorId);

                atribuicaoSupervisorEscolaDto.UESIds.Clear();
                await AjustarRegistrosExistentes(atribuicaoSupervisorEscolaDto, atribuicaoExistentes);

                throw new NegocioException($"O supervisor {atribuicaoSupervisorEscolaDto.SupervisorId} não é valido para essa atribuição.");
            }
        }

        private void AtribuirEscolas(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto)
        {
            if (atribuicaoSupervisorEscolaDto.UESIds != null)
            {
                foreach (var codigoEscolaDto in atribuicaoSupervisorEscolaDto.UESIds)
                {
                    repositorioSupervisorEscolaDre.Salvar(new SupervisorEscolaDre()
                    {
                        DreId = atribuicaoSupervisorEscolaDto.DreId,
                        SupervisorId = atribuicaoSupervisorEscolaDto.SupervisorId,
                        EscolaId = codigoEscolaDto
                    });
                }
            }
        }

        private async Task AjustarRegistrosExistentes(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto, System.Collections.Generic.IEnumerable<SupervisorEscolasDreDto> escolasAtribuidas)
        {
            if (escolasAtribuidas != null)
            {
                foreach (var atribuicao in escolasAtribuidas)
                {
                    if (atribuicaoSupervisorEscolaDto.UESIds == null || (!atribuicaoSupervisorEscolaDto.UESIds.Contains(atribuicao.EscolaId) && !atribuicao.Excluido))
                        await repositorioSupervisorEscolaDre.RemoverLogico(atribuicao.Id);
                    else if (atribuicaoSupervisorEscolaDto.UESIds.Contains(atribuicao.EscolaId) && atribuicao.Excluido)
                    {
                        var supervisorEscolaDre = repositorioSupervisorEscolaDre
                            .ObterPorId(atribuicao.Id);

                        supervisorEscolaDre.Excluido = false;

                        await repositorioSupervisorEscolaDre
                            .SalvarAsync(supervisorEscolaDre);
                    }

                    atribuicaoSupervisorEscolaDto
                        .UESIds.Remove(atribuicao.EscolaId);
                }
            }
        }
    }
}