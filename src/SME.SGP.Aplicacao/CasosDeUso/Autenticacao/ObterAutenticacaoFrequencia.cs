using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAutenticacaoFrequencia : AbstractUseCase, IObterAutenticacaoFrequencia
    {
        private readonly IComandosUsuario comandoUsuario;
        private readonly IRepositorioCache repositorioCache;

        public ObterAutenticacaoFrequencia(IMediator mediator, IComandosUsuario comandoUsuario, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.comandoUsuario = comandoUsuario ?? throw new ArgumentNullException(nameof(comandoUsuario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<UsuarioAutenticacaoFrequenciaRetornoDto> Executar(Guid guid)
        {
            var autenticacaoFrequenciaDto = await ObterUsuarioAutenticacaoFrequenciaCache(guid);

            var usuarioAutenticacao = await comandoUsuario.ObterAutenticacao(autenticacaoFrequenciaDto.UsuarioAutenticacao, autenticacaoFrequenciaDto.Rf);
            var retorno = new UsuarioAutenticacaoFrequenciaRetornoDto(usuarioAutenticacao, autenticacaoFrequenciaDto.Turma, autenticacaoFrequenciaDto.ComponenteCurricularCodigo);

            await RemoverUsuarioAutenticacaoFrequenciaCache(guid);
            return retorno;
        }

        private async Task<AutenticacaoFrequenciaDto> ObterUsuarioAutenticacaoFrequenciaCache(Guid guid)
        {
            var chaveCache = string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA, guid);
            var autenticacaoFrequenciaDtoCache = await repositorioCache.ObterAsync(chaveCache);

            if (string.IsNullOrWhiteSpace(autenticacaoFrequenciaDtoCache))
                throw new NegocioException("Guid de autenticação frequência inválido.");

            return JsonConvert.DeserializeObject<AutenticacaoFrequenciaDto>(autenticacaoFrequenciaDtoCache);

        }

        private async Task RemoverUsuarioAutenticacaoFrequenciaCache(Guid guid)
        {
            var chaveCache = string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA, guid);
            await mediator.Send(new RemoverChaveCacheCommand(chaveCache));
        }
    }
}
