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
            var mensagem = "Erro ao atribuir responsável.";
            var atribuidoComSucesso = false;
            try
            {
                // Etapa 1: Validação das regras de negócio. Lança exceção em caso de falha.
                await ValidarRegrasDeNegocio(atribuicaoResponsavelUe);

                // Etapa 2: Sincronização das atribuições.
                await SincronizarAtribuicoes(atribuicaoResponsavelUe);

                mensagem = "Atribuição salva com sucesso.";
                atribuidoComSucesso = true;
            }
            catch (ValidacaoCriticaRegraNegocioException ex)
            {
                await RemoverTodasAtribuicoesDoResponsavel(atribuicaoResponsavelUe);
                mensagem = ex.Message;
            }
            catch (NegocioException ex)
            {
                mensagem = ex.Message;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao atribuir responsável", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
            }

            return new SalvarAtribuicaoResponsavelStatus { AtribuidoComSucesso = atribuidoComSucesso, Mensagem = mensagem };
        }

        private async Task ValidarRegrasDeNegocio(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            // Validações foram quebradas em métodos privados com responsabilidades únicas.
            await ValidarEntidades(atribuicaoDto);
            await ValidarConflitosDeAtribuicao(atribuicaoDto);
            await ValidarResponsavel(atribuicaoDto);
        }

        private async Task ValidarEntidades(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            var dre = await mediator.Send(new ObterDREIdPorCodigoQuery(atribuicaoDto.DreId));
            if (dre < 1)
                throw new NegocioException($"A DRE {atribuicaoDto.DreId} não foi localizada.");

            foreach (var ueId in atribuicaoDto.UesIds)
            {
                var ueLocalizada = await mediator.Send(new ObterUeComDrePorCodigoQuery(ueId)) ??
                                   throw new NegocioException($"A UE {ueId} não foi localizada.");

                if (!ueLocalizada.Dre.CodigoDre.Equals(atribuicaoDto.DreId))
                    throw new NegocioException($"A UE {ueId} não pertence a DRE {atribuicaoDto.DreId}.");
            }
        }

        private async Task ValidarConflitosDeAtribuicao(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            foreach (var ueId in atribuicaoDto.UesIds)
            {
                var existeConflito = await repositorioSupervisorEscolaDre.VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(
                    (int)atribuicaoDto.TipoResponsavelAtribuicao, ueId, atribuicaoDto.DreId, atribuicaoDto.ResponsavelId);

                if (existeConflito > 0)
                {
                    var ueJaAtribuida = await mediator.Send(new ObterUeComDrePorCodigoQuery(ueId));
                    throw new NegocioException($"A UE {ueJaAtribuida.TipoEscola.ShortName()} {ueJaAtribuida.Nome} já está atribuída para outro responsável com o tipo {atribuicaoDto.TipoResponsavelAtribuicao.Name()}.");
                }
            }
        }

        private async Task ValidarResponsavel(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            var responsaveisValidos = await ObterResponsaveisEolOuCoreSSO(atribuicaoDto.DreId, atribuicaoDto.TipoResponsavelAtribuicao);
            if (!responsaveisValidos.Any(r => r.CodigoRfOuLogin.Equals(atribuicaoDto.ResponsavelId)))
            {
                // Apenas lança a exceção. A responsabilidade de remover as atribuições foi movida para o método 'Executar'.
                throw new ValidacaoCriticaRegraNegocioException($"O supervisor {atribuicaoDto.ResponsavelId} não é valido para essa atribuição.");
            }
        }

        private async Task SincronizarAtribuicoes(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            var atribuicoesAtuais = await repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(atribuicaoDto.DreId, atribuicaoDto.ResponsavelId, true);
            var uesAtuaisIds = atribuicoesAtuais.Select(a => a.EscolaId).ToList();
            var uesNovasIds = atribuicaoDto.UesIds;

            var uesParaAdicionar = uesNovasIds.Except(uesAtuaisIds).ToList();
            var uesParaReativar = atribuicoesAtuais.Where(a => a.AtribuicaoExcluida && uesNovasIds.Contains(a.EscolaId)).ToList();
            var uesParaRemover = atribuicoesAtuais.Where(a => !a.AtribuicaoExcluida && !uesNovasIds.Contains(a.EscolaId)).ToList();

            // Executa as operações de persistência de forma clara.
            AdicionarNovasAtribuicoes(uesParaAdicionar, atribuicaoDto);
            await ReativarAtribuicoes(uesParaReativar);
            RemoverAtribuicoes(uesParaRemover);
        }

        private void AdicionarNovasAtribuicoes(List<string> uesParaAdicionar, AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            foreach (var ueId in uesParaAdicionar)
            {
                var novaAtribuicao = new SupervisorEscolaDre
                {
                    DreId = atribuicaoDto.DreId,
                    SupervisorId = atribuicaoDto.ResponsavelId,
                    EscolaId = ueId,
                    Tipo = (int)atribuicaoDto.TipoResponsavelAtribuicao
                };
                repositorioSupervisorEscolaDre.Salvar(novaAtribuicao);
            }
        }

        private async Task ReativarAtribuicoes(List<SupervisorEscolasDreDto> uesParaReativar)
        {
            foreach (var atribuicao in uesParaReativar)
            {
                var registroParaReativar = await repositorioSupervisorEscolaDre.ObterPorIdAsync(atribuicao.AtribuicaoSupervisorId);
                if (registroParaReativar != null)
                {
                    registroParaReativar.Excluido = false;
                    await repositorioSupervisorEscolaDre.SalvarAsync(registroParaReativar);
                }
            }
        }

        private void RemoverAtribuicoes(List<SupervisorEscolasDreDto> uesParaRemover)
        {
            foreach (var atribuicao in uesParaRemover)
            {
                repositorioSupervisorEscolaDre.Remover(atribuicao.AtribuicaoSupervisorId);
            }
        }

        private async Task RemoverTodasAtribuicoesDoResponsavel(AtribuicaoResponsavelUEDto atribuicaoDto)
        {
            var atribuicoesExistentes = await repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(atribuicaoDto.DreId, atribuicaoDto.ResponsavelId, false);
            RemoverAtribuicoes(atribuicoesExistentes.ToList());
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
