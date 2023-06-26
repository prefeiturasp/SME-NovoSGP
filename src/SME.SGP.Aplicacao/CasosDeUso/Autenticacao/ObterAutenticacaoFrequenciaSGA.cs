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
    public class ObterAutenticacaoFrequenciaSGA : AbstractUseCase, IObterAutenticacaoFrequenciaSGA
    {
        private readonly IComandosUsuario comandoUsuario;
        private readonly IRepositorioCache repositorioCache;

        public ObterAutenticacaoFrequenciaSGA(IMediator mediator, IComandosUsuario comandoUsuario, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.comandoUsuario = comandoUsuario ?? throw new ArgumentNullException(nameof(comandoUsuario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<UsuarioAutenticacaoFrequenciaSGARetornoDto> Executar(Guid guid)
        {
            var autenticacaoFrequenciaSGADto = await ObterUsuarioAutenticacaoFrequenciaCache(guid);

            var usuarioAutenticacao = await comandoUsuario.ObterAutenticacao(autenticacaoFrequenciaSGADto.usuarioAutenticado, autenticacaoFrequenciaSGADto.Rf);
            var retorno = new UsuarioAutenticacaoFrequenciaSGARetornoDto(usuarioAutenticacao, autenticacaoFrequenciaSGADto.Turma, autenticacaoFrequenciaSGADto.ComponenteCurricularCodigo);
            return retorno;
        }

        private async Task<AutenticacaoFrequenciaSGADto> ObterUsuarioAutenticacaoFrequenciaCache(Guid guid)
        {
            var chaveCache = string.Format(NomeChaveCache.CHAVE_AUTENTICACAO_FREQUENCIA_SGA, guid);
            var autenticacaoFrequenciaSGADtoCache = await repositorioCache.ObterAsync(chaveCache);

            if (string.IsNullOrWhiteSpace(autenticacaoFrequenciaSGADtoCache))
                throw new NegocioException("Guid de autenticação frequência inválido.");

            return JsonConvert.DeserializeObject<AutenticacaoFrequenciaSGADto>(autenticacaoFrequenciaSGADtoCache);

        }
    }
}
