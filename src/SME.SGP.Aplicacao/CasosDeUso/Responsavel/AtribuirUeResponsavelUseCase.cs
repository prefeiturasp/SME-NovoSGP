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

        public async Task<SalvarAtribuicaoResponsavelStatus> Executar(AtribuicaoResponsavelUEDto atribuicaoResponsavelUe)
        {
            try
            {
                var validacao = await ValidarDados(atribuicaoResponsavelUe);

                if (string.IsNullOrEmpty(validacao))
                {
                    var escolasAtribuidas = await repositorioSupervisorEscolaDre
                            .ObtemPorDreESupervisor(atribuicaoResponsavelUe.DreId, atribuicaoResponsavelUe.ResponsavelId, false);

                    await AjustarRegistrosExistentes(atribuicaoResponsavelUe, escolasAtribuidas);
                    AtribuirEscolas(atribuicaoResponsavelUe);
                }
                return new SalvarAtribuicaoResponsavelStatus { AtribuidoComSucesso = string.IsNullOrEmpty(validacao), Mensagem = validacao };
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao Atribuir responsável", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return new SalvarAtribuicaoResponsavelStatus { AtribuidoComSucesso = false, Mensagem = "Erro ao Atribuir responsável" };
            }
        }

        private async Task<string> ValidarDados(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto)
        {
            var retorno = string.Empty;
            foreach (var ueCodigo in atribuicaoSupervisorEscolaDto.UesIds)
            {
                var existe = await repositorioSupervisorEscolaDre.VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe((int)atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao, ueCodigo, atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.ResponsavelId);
                if (existe > 0)
                {
                    var ueJaAtribuida = await mediator.Send(new ObterUeComDrePorCodigoQuery(ueCodigo));
                    retorno = $"A UE {ueJaAtribuida.TipoEscola.ShortName()} {ueJaAtribuida.Nome} já está atribuída para outro responsável com o tipo {atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao.Name()}.";
                }
            }
            var dre = await mediator.Send(new ObterDREIdPorCodigoQuery(atribuicaoSupervisorEscolaDto.DreId));

            if (dre < 1)
                retorno = $"A DRE {atribuicaoSupervisorEscolaDto.DreId} não foi localizada.";


            foreach (var ue in atribuicaoSupervisorEscolaDto.UesIds)
            {
                var ueLocalizada = await mediator.Send(new ObterUeComDrePorCodigoQuery(ue));

                if (ueLocalizada.EhNulo())
                    retorno = $"A UE {ue} não foi localizada.";

                if (!ueLocalizada.Dre.CodigoDre.Equals(atribuicaoSupervisorEscolaDto.DreId))
                    retorno = $"A UE {ue} não pertence a DRE {atribuicaoSupervisorEscolaDto.DreId}.";
            }

            var responsaveisEolOuCoreSSO = await ObterResponsaveisEolOuCoreSSO(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.TipoResponsavelAtribuicao);

            if (!responsaveisEolOuCoreSSO.Any(s => s.CodigoRfOuLogin.Equals(atribuicaoSupervisorEscolaDto.ResponsavelId)))
            {
                var atribuicaoExistentes = await repositorioSupervisorEscolaDre
                    .ObtemPorDreESupervisor(atribuicaoSupervisorEscolaDto.DreId, atribuicaoSupervisorEscolaDto.ResponsavelId);

                atribuicaoSupervisorEscolaDto.UesIds.Clear();
                await AjustarRegistrosExistentes(atribuicaoSupervisorEscolaDto, atribuicaoExistentes);

                retorno = $"O supervisor {atribuicaoSupervisorEscolaDto.ResponsavelId} não é valido para essa atribuição.";
            }
            return retorno;
        }

        private void AtribuirEscolas(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto)
        {
            if (atribuicaoSupervisorEscolaDto.UesIds.NaoEhNulo())
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

        private async Task AjustarRegistrosExistentes(AtribuicaoResponsavelUEDto atribuicaoSupervisorEscolaDto, IEnumerable<SupervisorEscolasDreDto> escolasAtribuidas)
        {
            if (escolasAtribuidas.NaoEhNulo())
            {
                foreach (var atribuicao in escolasAtribuidas)
                {
                    if (atribuicaoSupervisorEscolaDto.UesIds.EhNulo() || (!atribuicaoSupervisorEscolaDto.UesIds.Contains(atribuicao.EscolaId) && !atribuicao.AtribuicaoExcluida))
                        repositorioSupervisorEscolaDre.Remover(atribuicao.AtribuicaoSupervisorId);

                    else if (atribuicaoSupervisorEscolaDto.UesIds.Contains(atribuicao.EscolaId) && atribuicao.AtribuicaoExcluida)
                    {
                        var supervisorEscolaDre = repositorioSupervisorEscolaDre
                            .ObterPorId(atribuicao.AtribuicaoSupervisorId);

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
