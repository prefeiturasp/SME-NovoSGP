using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreUseCase : AbstractUseCase, IObterResponsaveisPorDreUseCase
    {
        public ObterResponsaveisPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterNomesResponsaveisAtribuidos(IEnumerable<SupervisorEscolasDreDto> responsaveisAtribuidos,
            TipoResponsavelAtribuicao? tipoResponsavelAtribuicao)
        {
            var listaNomesResponsaveisAtribuidos = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            if (responsaveisAtribuidos.EhNulo() || !responsaveisAtribuidos.Any())
                return listaNomesResponsaveisAtribuidos;

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var nomesFuncionariosAtribuidos = await mediator.Send(new ObterFuncionariosPorLoginsQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var funcionario in nomesFuncionariosAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRfOuLogin = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    {
                        var nomesServidoresAtribuidos = await mediator.Send(new ObterFuncionariosPorRFsQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var responsavel in nomesServidoresAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRfOuLogin = responsavel.CodigoRF,
                                NomeServidor = responsavel.Nome
                            });
                        }

                        break;
                    }
            }

            if (listaNomesResponsaveisAtribuidos.EhNulo() || !listaNomesResponsaveisAtribuidos.Any())
                throw new NegocioException("A API/EOL não retornou os nomes dos responsáveis atribuídos.");

            return await Task.FromResult(listaNomesResponsaveisAtribuidos);
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterResponsaveisEolOuCoreSSO(string dreCodigo, TipoResponsavelAtribuicao? tipoResponsavelAtribuicao)
        {
            var listaResponsaveis = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                    {
                        var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(dreCodigo, 29));

                        if (funcionariosEol.NaoEhNulo())
                        {
                            foreach (var funcionario in funcionariosEol)
                            {
                                listaResponsaveis.Add(new ResponsavelRetornoDto()
                                {
                                    CodigoRfOuLogin = funcionario.CodigoRf,
                                    NomeServidor = funcionario.NomeServidor
                                });
                            }
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

                        if (funcionariosUnidades.NaoEhNulo())
                        {
                            foreach (var funcionario in funcionariosUnidades)
                            {
                                listaResponsaveis.Add(new ResponsavelRetornoDto()
                                {
                                    CodigoRfOuLogin = funcionario.Login,
                                    NomeServidor = funcionario.NomeServidor
                                });
                            }
                        }

                        break;
                    }
                default:
                    var supervisoresEol = (await mediator.Send(new ObterSupervisoresPorDreEolQuery(dreCodigo))).ToList();

                    if(supervisoresEol.NaoEhNulo())
                    {
                        foreach (var supervisor in supervisoresEol)
                        {
                            listaResponsaveis.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRfOuLogin = supervisor.CodigoRf,
                                NomeServidor = supervisor.NomeServidor
                            });
                        }
                    }
                    

                    break;
            }

            return await Task.FromResult(listaResponsaveis);
        }

        public async Task<IEnumerable<SupervisorDto>> Executar(ObterResponsaveisPorDreDto filtro)
        {
            var listaResponsaveis = new List<SupervisorDto>();

            if ((int)(filtro?.TipoResponsavelAtribuicao ?? 0) == 0)
                return listaResponsaveis;

            var responsaveisEolOuCoreSSO = await ObterResponsaveisEolOuCoreSSO(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao);

            //-> Obtem os resposáveis já atribuidos
            var responsaveisAtribuidos = (await mediator.Send(new ObterResponsaveisPorDreQuery(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao)))
                .DistinctBy(c => c.SupervisorId);

            var nomesResponsaveisAtribuidos = await ObterNomesResponsaveisAtribuidos(responsaveisAtribuidos, filtro.TipoResponsavelAtribuicao);

            if (responsaveisEolOuCoreSSO.NaoEhNulo() && responsaveisEolOuCoreSSO.Any())
            {
                listaResponsaveis.AddRange(responsaveisEolOuCoreSSO?.Select(a => new SupervisorDto()
                {
                    SupervisorId = a.CodigoRfOuLogin,
                    SupervisorNome = a.NomeServidor
                }));
            }

            if (responsaveisAtribuidos.NaoEhNulo() && responsaveisAtribuidos.Any())
            {
                listaResponsaveis.AddRange(responsaveisAtribuidos?.Where(s => !responsaveisEolOuCoreSSO.Select(se => se.CodigoRfOuLogin).Contains(s.SupervisorId))?
                    .Select(a => new SupervisorDto()
                    {
                        SupervisorId = a.SupervisorId,
                        SupervisorNome = nomesResponsaveisAtribuidos?.FirstOrDefault(n => n.CodigoRfOuLogin == a.SupervisorId)?.NomeServidor
                    }));
            }

            return listaResponsaveis?.OrderBy(s => s.SupervisorNome).ThenBy(c => c.SupervisorId);
        }
    }
}
