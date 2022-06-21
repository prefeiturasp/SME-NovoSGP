using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirUeResponsavelUseCase : AbstractUseCase, IAtribuirUeResponsavelUseCase
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        public AtribuirUeResponsavelUseCase(IMediator mediator,
            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre) : base(mediator)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<bool> Executar(AtribuicaoResponsavelUEDto atribuicaoResponsavelUe)
        {
            try
            {
                await ValidarDados(atribuicaoResponsavelUe);

                var escolasAtribuidas = await repositorioSupervisorEscolaDre
                    .ObtemPorDreESupervisor(atribuicaoResponsavelUe.DreId, atribuicaoResponsavelUe.ResponsavelId, true);

                await AjustarRegistrosExistentes(atribuicaoResponsavelUe, escolasAtribuidas);
                AtribuirEscolas(atribuicaoResponsavelUe);
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao Atribuir responsável", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private async Task ValidarDados(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto)
        {
            var dre = await mediator.Send(new ObterDREIdPorCodigoQuery(atribuicaoSupervisorEscolaDto.DreId));

            if (dre < 1)
                throw new NegocioException($"A DRE {atribuicaoSupervisorEscolaDto.DreId} não foi localizada.");

            atribuicaoSupervisorEscolaDto.UesIds
                .ForEach(ue =>
                {
                    var ueLocalizada = mediator.Send(new ObterUeComDrePorCodigoQuery(ue)).Result;

                    if (ueLocalizada == null)
                        throw new NegocioException($"A UE {ue} não foi localizada.");

                    if (!ueLocalizada.Dre.CodigoDre.Equals(atribuicaoSupervisorEscolaDto.DreId))
                        throw new NegocioException($"A UE {ue} não pertence a DRE {atribuicaoSupervisorEscolaDto.DreId}.");
                });

            var responsaveisEolOuCoreSSO = await ObterResponsaveisEolOuCoreSSO(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao);

            if (!responsaveisEolOuCoreSSO.Any(s => s.CodigoRfOuLogin.Equals(atribuicaoSupervisorEscolaDto.ResponsavelId)))
            {
                var atribuicaoExistentes = await repositorioSupervisorEscolaDre
                    .ObtemPorDreESupervisor(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.ResponsavelId);

                atribuicaoSupervisorEscolaDto.UesIds.Clear();
                await AjustarRegistrosExistentes(atribuicaoSupervisorEscolaDto, atribuicaoExistentes);

                throw new NegocioException($"O supervisor {atribuicaoSupervisorEscolaDto.ResponsavelId} não é valido para essa atribuição.");
            }
        }

        private void AtribuirEscolas(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto)
        {
            if (atribuicaoSupervisorEscolaDto.UesIds != null)
            {
                foreach (var codigoEscolaDto in atribuicaoSupervisorEscolaDto.UesIds)
                {
                    repositorioSupervisorEscolaDre.Salvar(new SupervisorEscolaDre()
                    {
                        DreId = atribuicaoSupervisorEscolaDto.DreId,
                        SupervisorId = atribuicaoSupervisorEscolaDto.ResponsavelId,
                        EscolaId = codigoEscolaDto,
                        Tipo = (int)atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao
                    });
                }
            }
        }

        private async Task AjustarRegistrosExistentes(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto,
            IEnumerable<SupervisorEscolasDreDto> escolasAtribuidas)
        {
            if (escolasAtribuidas != null)
            {
                foreach (var atribuicao in escolasAtribuidas)
                {
                    if (atribuicaoSupervisorEscolaDto.UesIds == null || (!atribuicaoSupervisorEscolaDto.UesIds.Contains(atribuicao.EscolaId) && !atribuicao.AtribuicaoExcluida))
                        await repositorioSupervisorEscolaDre.RemoverLogico(atribuicao.Id);

                    else if (atribuicaoSupervisorEscolaDto.UesIds.Contains(atribuicao.EscolaId) && atribuicao.AtribuicaoExcluida)
                    {
                        var supervisorEscolaDre = repositorioSupervisorEscolaDre
                            .ObterPorId(atribuicao.Id);

                        supervisorEscolaDre.Excluido = false;
                        supervisorEscolaDre.SupervisorId = atribuicaoSupervisorEscolaDto.ResponsavelId;
                        supervisorEscolaDre.Tipo = atribuicao.TipoAtribuicao;

                        await repositorioSupervisorEscolaDre.SalvarAsync(supervisorEscolaDre);
                    }
                    atribuicaoSupervisorEscolaDto.UesIds.Remove(atribuicao.EscolaId);
                }
            }
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterResponsaveisEolOuCoreSSO(string dreCodigo, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
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
                                CodigoRfOuLogin = funcionario.CodigoRf,
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
                                CodigoRfOuLogin = funcionario.Login,
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
                            CodigoRfOuLogin = supervisor.CodigoRf,
                            NomeServidor = supervisor.NomeServidor
                        });
                    }

                    break;
            }

            return await Task.FromResult(listaResponsaveis);
        }
    }
}
