using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SupervisorAtribuirUeUseCase : AbstractUseCase, ISupervisorAtribuirUeUseCase
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IUnitOfWork unitOfWork;

        public SupervisorAtribuirUeUseCase(IMediator mediator,
            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(AtribuicaoSupervisorUEDto atribuicaoSupervisorUe)
        {
            await ValidarDados(atribuicaoSupervisorUe);

            var escolasAtribuidas = await repositorioSupervisorEscolaDre
                .ObtemPorDreESupervisor(atribuicaoSupervisorUe.DreId, atribuicaoSupervisorUe.SupervisorId, true);

            unitOfWork.IniciarTransacao();
            try
            {
                await AjustarRegistrosExistentes(atribuicaoSupervisorUe, escolasAtribuidas);
                AtribuirEscolas(atribuicaoSupervisorUe);
                unitOfWork.PersistirTransacao();
            } 
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return await Task.FromResult(true);
        }

        private async Task ValidarDados(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto)
        {
            var dre = await mediator.Send(new ObterDREIdPorCodigoQuery(atribuicaoSupervisorEscolaDto.DreId));

            if (dre < 1)
                throw new NegocioException($"A DRE {atribuicaoSupervisorEscolaDto.DreId} não foi localizada.");

            var responsaveisEol_CoreSSO = await ObterResponsaveisEol_CoreSSO(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao);

            atribuicaoSupervisorEscolaDto.UESIds
                .ForEach(ue =>
                {
                    var ueLocalizada = mediator.Send(new ObterUeComDrePorCodigoQuery(ue)).Result;

                    if (ueLocalizada == null)
                        throw new NegocioException($"A UE {ue} não foi localizada.");

                    if (!ueLocalizada.Dre.CodigoDre.Equals(atribuicaoSupervisorEscolaDto.DreId))
                        throw new NegocioException($"A UE {ue} não pertence a DRE {atribuicaoSupervisorEscolaDto.DreId}.");
                });

            if (!responsaveisEol_CoreSSO.Any(s => s.CodigoRf_Login.Equals(atribuicaoSupervisorEscolaDto.SupervisorId)))
            {
                var atribuicaoExistentes = await repositorioSupervisorEscolaDre
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
                        EscolaId = codigoEscolaDto,
                        Tipo = (int)atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao
                    });
                }
            }
        }

        private async Task AjustarRegistrosExistentes(AtribuicaoSupervisorUEDto atribuicaoSupervisorEscolaDto,
            IEnumerable<SupervisorEscolasDreDto> escolasAtribuidas)
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
                        supervisorEscolaDre.Tipo = atribuicao.Tipo;

                        await repositorioSupervisorEscolaDre.SalvarAsync(supervisorEscolaDre);
                    }

                    atribuicaoSupervisorEscolaDto.UESIds.Remove(atribuicao.EscolaId);
                }
            }
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterResponsaveisEol_CoreSSO(string dreCodigo, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var listaResponsaveis = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                    {
                        var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(dreCodigo, 29));

                        foreach (var funcionario in funcionariosEol)
                        {
                            listaResponsaveis.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.CodigoRf,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var perfil = Perfis.PERFIL_PSICOLOGO_ESCOLAR;

                        if (tipoResponsavelAtribuicao == TipoResponsavelAtribuicao.Psicopedagogo)
                            perfil = Perfis.PERFIL_PSICOPEDAGOGO;
                        else if (tipoResponsavelAtribuicao == TipoResponsavelAtribuicao.AssistenteSocial)
                            perfil = Perfis.PERFIL_ASSISTENTE_SOCIAL;

                        var funcionariosUnidades = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(dreCodigo,
                            new List<Guid> { perfil }))).ToList();

                        foreach (var funcionario in funcionariosUnidades)
                        {
                            listaResponsaveis.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    var supervisoresEol = (await mediator.Send(new ObterSupervisoresPorDreEolQuery(dreCodigo))).ToList();

                    foreach (var supervisor in supervisoresEol)
                    {
                        listaResponsaveis.Add(new ResponsavelRetornoDto()
                        {
                            CodigoRf_Login = supervisor.CodigoRf,
                            NomeServidor = supervisor.NomeServidor
                        });
                    }

                    break;
            }

            return await Task.FromResult(listaResponsaveis);
        }
    }
}
