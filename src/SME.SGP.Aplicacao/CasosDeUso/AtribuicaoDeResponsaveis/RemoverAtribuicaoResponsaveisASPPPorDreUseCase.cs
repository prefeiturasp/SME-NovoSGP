using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisASPPPorDreUseCase : IRemoverAtribuicaoResponsaveisASPPPorDreUseCase
    {
        private readonly IMediator mediator;

        public RemoverAtribuicaoResponsaveisASPPPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var dre = param.ObterObjetoMensagem<string>();
                var assitenteSocialSGP = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(dre, TipoResponsavelAtribuicao.AssistenteSocial));
                var psicologosSGP = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(dre, TipoResponsavelAtribuicao.PsicologoEscolar));
                var psicoPedagogosSGP = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(dre, TipoResponsavelAtribuicao.Psicopedagogo));

                var funcionariosSGP = new List<SupervisorEscolasDreDto>();

                if (assitenteSocialSGP.NaoEhNulo() && assitenteSocialSGP.Any())
                    funcionariosSGP.AddRange(assitenteSocialSGP);

                if (psicologosSGP.NaoEhNulo() && psicologosSGP.Any())
                    funcionariosSGP.AddRange(psicologosSGP);

                if (psicoPedagogosSGP.NaoEhNulo() && psicoPedagogosSGP.Any())
                    funcionariosSGP.AddRange(psicoPedagogosSGP);

                if (funcionariosSGP.Any())
                {
                    var funcionariosEOL = new List<UsuarioEolRetornoDto>();

                    var funcionariosPsicoloEscolarEOL = await mediator.Send(new ObterFuncionarioCoreSSOPorPerfilDreQuery(Perfis.PERFIL_PSICOLOGO_ESCOLAR, dre));
                    var funcionariosPsicoPedagogosEOL = await mediator.Send(new ObterFuncionarioCoreSSOPorPerfilDreQuery(Perfis.PERFIL_PSICOPEDAGOGO, dre));
                    var funcionariosAssistenteSocialEOL = await mediator.Send(new ObterFuncionarioCoreSSOPorPerfilDreQuery(Perfis.PERFIL_ASSISTENTE_SOCIAL, dre));

                    if (funcionariosPsicoloEscolarEOL.NaoEhNulo() && funcionariosPsicoloEscolarEOL.Any())
                        funcionariosEOL.AddRange(funcionariosPsicoloEscolarEOL);

                    if (funcionariosPsicoPedagogosEOL.NaoEhNulo() && funcionariosPsicoPedagogosEOL.Any())
                        funcionariosEOL.AddRange(funcionariosPsicoPedagogosEOL);

                    if (funcionariosAssistenteSocialEOL.NaoEhNulo() && funcionariosAssistenteSocialEOL.Any())
                        funcionariosEOL.AddRange(funcionariosAssistenteSocialEOL);

                    return await RemoverASPPCoreSSoSemAtribuicao(funcionariosSGP, funcionariosEOL);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel do Core SSO (Assistente social, Psicopedagogo, Psicologo) por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private async Task<bool> RemoverASPPCoreSSoSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> responsaveisSGP, IEnumerable<UsuarioEolRetornoDto> responsaveisEol)
        {
            var listaAsspSemAtribuicao = new List<SupervisorEscolasDreDto>();

            var assitenteSocialEscolasSemAtribuicao = responsaveisSGP.Where(s => s.TipoAtribuicao == (int) TipoResponsavelAtribuicao.AssistenteSocial && !responsaveisEol.Select(e => e.Login.ToString()).Contains(s.SupervisorId));
            var psicologosEscolasSemAtribuicao = responsaveisSGP.Where(s => s.TipoAtribuicao == (int) TipoResponsavelAtribuicao.PsicologoEscolar && !responsaveisEol.Select(e => e.Login.ToString()).Contains(s.SupervisorId));
            var psicopedagogosEscolasSemAtribuicao = responsaveisSGP.Where(s => s.TipoAtribuicao == (int) TipoResponsavelAtribuicao.Psicopedagogo && !responsaveisEol.Select(e => e.Login.ToString()).Contains(s.SupervisorId));

            if (assitenteSocialEscolasSemAtribuicao.NaoEhNulo() && assitenteSocialEscolasSemAtribuicao.Any())
                listaAsspSemAtribuicao.AddRange(assitenteSocialEscolasSemAtribuicao);

            if (psicologosEscolasSemAtribuicao.NaoEhNulo() && psicologosEscolasSemAtribuicao.Any())
                listaAsspSemAtribuicao.AddRange(psicologosEscolasSemAtribuicao);

            if (psicopedagogosEscolasSemAtribuicao.NaoEhNulo() && psicopedagogosEscolasSemAtribuicao.Any())
                listaAsspSemAtribuicao.AddRange(psicopedagogosEscolasSemAtribuicao);

            foreach (var supervisor in listaAsspSemAtribuicao)
            {
                var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                await mediator.Send(new RemoverAtribuicaoSupervisorCommand(supervisorEntidadeExclusao));
            }

            return true;
        }

        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.AtribuicaoSupervisorId,
                Excluido = dto.AtribuicaoExcluida,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.TipoAtribuicao
            };
        }
    }
}