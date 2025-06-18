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
            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                        return (await ObterFuncionariosDreCargo(dreCodigo, 29));   
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                        return (await ObterFuncionariosDrePerfis(dreCodigo, Perfis.PERFIL_PSICOLOGO_ESCOLAR));
                case TipoResponsavelAtribuicao.Psicopedagogo:
                    return (await ObterFuncionariosDrePerfis(dreCodigo, Perfis.PERFIL_PSICOPEDAGOGO));
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    return (await ObterFuncionariosDrePerfis(dreCodigo, Perfis.PERFIL_ASSISTENTE_SOCIAL));
                default:
                    return (await ObterSupervisoresDre(dreCodigo));
            }
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterSupervisoresDre(string codigoDRE)
        {
            var supervisoresEol = await mediator.Send(new ObterSupervisoresPorDreEolQuery(codigoDRE));
            if (supervisoresEol.PossuiRegistros())
                return supervisoresEol.Select(spr => new ResponsavelRetornoDto()
                {
                    CodigoRfOuLogin = spr.CodigoRf,
                    NomeServidor = spr.NomeServidor
                });
            return Enumerable.Empty<ResponsavelRetornoDto>();
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterFuncionariosDrePerfis(string codigoDRE, Guid perfil)
        {
            var funcionariosUnidades = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDRE,
                            new List<Guid> { perfil }));

            if (funcionariosUnidades.PossuiRegistros())
                return funcionariosUnidades.Select(fnc => new ResponsavelRetornoDto()
                {
                    CodigoRfOuLogin = fnc.Login,
                    NomeServidor = fnc.NomeServidor
                });
            return Enumerable.Empty<ResponsavelRetornoDto>();
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterFuncionariosDreCargo(string codigoDRE, int codigoCargo)
        {
            var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(codigoDRE, codigoCargo));
            if (funcionariosEol.PossuiRegistros())
                return funcionariosEol.Select(fnc => new ResponsavelRetornoDto()
                {
                    CodigoRfOuLogin = fnc.CodigoRf,
                    NomeServidor = fnc.NomeServidor
                });
            return Enumerable.Empty<ResponsavelRetornoDto>();
        }

        public async Task<IEnumerable<SupervisorDto>> Executar(ObterResponsaveisPorDreDto filtro)
        {
            var listaResponsaveis = new List<SupervisorDto>();

            if ((int)(filtro?.TipoResponsavelAtribuicao ?? 0) == 0)
                return listaResponsaveis;

            var responsaveisEolOuCoreSSO = await ObterResponsaveisEolOuCoreSSO(filtro?.DreCodigo, filtro?.TipoResponsavelAtribuicao);

            //-> Obtem os resposáveis já atribuidos
            var responsaveisAtribuidos = (await mediator.Send(new ObterResponsaveisPorDreQuery(filtro?.DreCodigo, filtro?.TipoResponsavelAtribuicao)))
                .DistinctBy(c => c.SupervisorId);

            var nomesResponsaveisAtribuidos = await ObterNomesResponsaveisAtribuidos(responsaveisAtribuidos, filtro?.TipoResponsavelAtribuicao);

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
