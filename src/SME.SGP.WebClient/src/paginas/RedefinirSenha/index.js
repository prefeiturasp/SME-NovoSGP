import React, { useState, useRef, useEffect, useLayoutEffect } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Form } from 'formik';
import LogoDoSgp from '~/recursos/LogoDoSgp.svg';
import { Base, Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import RedefinirSenhaServico from '~/servicos/Paginas/ServicoRedefinirSenha';
import {
  Nav,
  Logo,
  Div,
  Container,
  Texto,
  Titulo,
  Rotulo,
  CampoTexto,
  Validacoes,
  Itens,
  Icone,
  MensagemErro,
} from './index.css';
import { URL_LOGIN, URL_RECUPERARSENHA, URL_HOME } from '~/constantes/url';
import ServicoPrimeiroAcesso from '~/servicos/Paginas/ServicoPrimeiroAcesso';
import { salvarDadosLogin, Deslogar } from '~/redux/modulos/usuario/actions';
import { store } from '~/redux';
import Erro from '../RecuperarSenha/erro';
import { setMenusPermissoes } from '~/servicos/servico-navegacao';
import { obterMeusDados } from '~/servicos/Paginas/ServicoUsuario';

const Item = styled.li`
  ${props => props.status === true && `color: ${Base.Verde}`};
  ${props => props.status === false && `color: ${Base.VermelhoNotificacao}`};
  font-weight: normal;
`;

const RedefinirSenha = props => {
  const [senhas, setSenhas] = useState({
    senha: '',
    confirmarSenha: '',
  });

  const [tokenValidado, setTokenValidado] = useState(false);
  const [erroGeral, setErroGeral] = useState('');
  const [tokenExpirado, setTokenExpirado] = useState(false);

  const { senha, confirmarSenha } = senhas;
  const token = props.match && props.match.params && props.match.params.token;

  const [validacoes, setValidacoes] = useState({
    maiuscula: '',
    minuscula: '',
    algarismo: '',
    simbolo: '',
    acentos: '',
    tamanho: '',
    iguais: '',
    espacoBranco: '',
  });

  const montaIcone = status => {
    let estilo = 'd-none';
    if (typeof status === 'boolean') {
      if (status) estilo = 'd-inline-block fa fa-check';
      else estilo = 'd-inline-block fa fa-times';
    }
    return <Icone className={estilo} />;
  };

  const inputSenhaRef = useRef();
  const inputConfSenhaRef = useRef();
  const { logado, usuario } = useSelector(state => state.usuario);
  const modificarSenha = useSelector(state => state.usuario.modificarSenha);

  const trataAcaoTeclado = e => {
    if (!token && e.code === 'F5') {
      store.dispatch(Deslogar());
      history.push(URL_LOGIN);
    }
  };

  useLayoutEffect(() => {
    if (!tokenValidado && !logado) validarToken();

    document.addEventListener('keydown', trataAcaoTeclado);
    return () => {
      document.removeEventListener('keydown', trataAcaoTeclado);
    };
  }, []);

  useEffect(() => {
    if (inputSenhaRef.current) inputSenhaRef.current.focus();
  }, [senha]);

  useEffect(() => {
    if (inputConfSenhaRef.current) inputConfSenhaRef.current.focus();
  }, [confirmarSenha]);

  const aoMudarSenha = e => {
    setErroGeral('');
    setSenhas({ ...senhas, senha: e.target.value });
    realizarValidacoes(e.target.value);
  };

  const validarToken = async () => {
    let tokenValido = true;
    if (!token) history.push(URL_LOGIN);
    if (token) tokenValido = await RedefinirSenhaServico.validarToken(token);

    if (!tokenValido) {
      setErroGeral(
        'Esse link expirou. Clique em continuar para solicitar um link novo.'
      );
    } else setTokenValidado(true);
  };

  const aoMudarConfSenha = e => {
    setSenhas({ ...senhas, confirmarSenha: e.target.value });
    setErroGeral('');

    const iguais = e.target.value === inputSenhaRef.current.value;
    setValidacoes({ ...validacoes, iguais });
  };

  const realizarValidacoes = valor => {
    const temMaiuscula = valor.match(/([A-Z])/);
    const temMinuscula = valor.match(/([a-z])/);
    const temAlgarismo = valor.match(/([0-9])/);
    const temSimbolo = valor.match(/([!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?])/);
    const temAcento = valor.match(/([À-ÖØ-öø-ÿ])/);
    const espacoBranco = valor.includes(' ');

    const iguais =
      inputConfSenhaRef.current.value === inputSenhaRef.current.value;

    const tamanho = valor.length >= 8 && valor.length <= 12;

    setValidacoes({
      maiuscula: !!temMaiuscula,
      minuscula: !!temMinuscula,
      algarismo: !!temAlgarismo || !!temSimbolo,
      simbolo: !!temSimbolo || !!temAlgarismo,
      acentuados: !temAcento,
      espacoBranco: !espacoBranco,
      tamanho: !!tamanho,
      iguais: !!iguais,
    });
  };

  const validarSeFormularioTemErro = () =>
    Object.entries(validacoes).filter(validacao => !validacao[1]).length > 0;

  const onClickSair = () => {
    if (modificarSenha) store.dispatch(Deslogar());
    history.push(URL_LOGIN);
  };

  const alterarSenha = async () => {
    if (!logado) {
      const requisicao = await RedefinirSenhaServico.redefinirSenha({
        token,
        novaSenha: senha,
      });

      if (requisicao.sucesso) history.push(URL_LOGIN);
      if (requisicao.tokenExpirado) setTokenExpirado(requisicao.tokenExpirado);

      setErroGeral(requisicao.erro);
    } else {
      const requisicao = await ServicoPrimeiroAcesso.alterarSenha({
        usuario,
        novaSenha: senha,
        confirmarSenha: senha,
      });
      if (requisicao.sucesso) {
        obterMeusDados();
        setMenusPermissoes();

        const rf = Number.isInteger(usuario * 1) ? usuario : '';
        store.dispatch(
          salvarDadosLogin({
            token: requisicao.resposta.data.token,
            rf,
            usuario,
            perfisUsuario: requisicao.resposta.data.perfisUsuario,
            modificarSenha: false,
            possuiPerfilSmeOuDre:
              requisicao.resposta.data.perfisUsuario.possuiPerfilSmeOuDre,
            possuiPerfilDre:
              requisicao.resposta.data.perfisUsuario.possuiPerfilDre,
            possuiPerfilSme:
              requisicao.resposta.data.perfisUsuario.possuiPerfilSme,
            ehProfessorCj: requisicao.resposta.data.perfisUsuario.ehProfessorCj,
            dataHoraExpiracao: requisicao.resposta.data.dataHoraExpiracao,
          })
        );
        history.push(URL_HOME);
      } else {
        setErroGeral(
          requisicao.status === 504
            ? 'Ocorreu um erro de comunicação com o servidor.'
            : requisicao.erro
        );
      }
    }
  };

  const aoClicarContinuar = () => {
    realizarValidacoes(inputSenhaRef.current.value);
    setErroGeral('');

    if (tokenExpirado) {
      history.push(URL_RECUPERARSENHA);
      return;
    }

    if (!validarSeFormularioTemErro()) alterarSenha();
  };

  const aoClicarContinuarExpirado = () => {
    if (token && !tokenValidado) history.push(URL_RECUPERARSENHA);
  };

  return (
    <>
      <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top py-0">
        <Logo
          src={LogoDoSgp}
          className="mx-auto"
          alt="Novo Sistema de Gestão Pedagógica"
        />
      </Nav>
      <Container className="container text-center shadow-sm mx-auto mt-4 rounded">
        <Div className="row">
          <Div className="col-xl-12 col-lg-12 col-md-12 col-sm-12">
            <Texto
              className="mx-auto"
              style={{ marginBottom: '70px', maxWidth: '560px' }}
            >
              {token && !tokenValidado ? (
                <>
                  <Titulo style={{ marginTop: '70px', marginBottom: '40px' }}>
                    Recuperação de Senha
                  </Titulo>
                  <Erro mensagem="Esse link expirou. Clique em continuar para solicitar um link novo." />
                  <Button
                    label="Continuar"
                    color={Colors.Roxo}
                    className="btn-block w-75 mx-auto mt-5"
                    onClick={aoClicarContinuarExpirado}
                    bold
                  />
                </>
              ) : (
                <>
                  <Titulo style={{ marginTop: '70px', marginBottom: '40px' }}>
                    Nova Senha
                  </Titulo>
                  <Div style={{ marginBottom: '60px' }}>
                    Identificamos que você ainda não definiu uma senha pessoal
                    para acesso ao SGP. Este passo é obrigatório para que você
                    tenha acesso ao sistema.
                  </Div>
                  <Div style={{ maxWidth: '295px' }} className="w-100 mx-auto">
                    <Form>
                      <Div
                        style={{ marginBottom: '30px' }}
                        className="form-group text-left"
                      >
                        <Rotulo htmlFor="senha">Nova Senha</Rotulo>
                        <CampoTexto
                          name="senha"
                          id="senha"
                          maxlength={50}
                          className="form-control form-control-lg rounded"
                          placeholder="Insira sua nova senha"
                          type="password"
                          value={senha}
                          ref={inputSenhaRef}
                          onChange={aoMudarSenha}
                        />
                      </Div>
                      <Div
                        style={{ marginBottom: '25px' }}
                        className="form-group text-left"
                      >
                        <Rotulo htmlFor="confirmacao">
                          Confirmação da Nova Senha
                        </Rotulo>
                        <CampoTexto
                          name="confirmacao"
                          id="confirmacao"
                          maxlength={50}
                          className="form-control form-control-lg rounded"
                          placeholder="Confirme sua nova senha"
                          type="password"
                          ref={inputConfSenhaRef}
                          onChange={aoMudarConfSenha}
                          icon
                        />
                      </Div>
                      <Validacoes
                        className="text-left"
                        style={{ marginBottom: '30px' }}
                      >
                        <Div style={{ lineHeight: '1.8' }}>
                          Requisitos de segurança da senha:
                        </Div>
                        <Itens className="list-unstyled">
                          <Item status={validacoes.maiuscula}>
                            Uma letra maiúscula
                            {montaIcone(validacoes.maiuscula)}
                          </Item>
                          <Item status={validacoes.minuscula}>
                            Uma letra minúscula
                            {montaIcone(validacoes.minuscula)}
                          </Item>
                          <Item status={validacoes.iguais}>
                            As senhas devem ser iguais
                            {montaIcone(validacoes.iguais)}
                          </Item>
                          <Item status={validacoes.espacoBranco}>
                            Não pode conter espaços em branco
                            {montaIcone(validacoes.espacoBranco)}
                          </Item>
                          <Item status={validacoes.acentuados}>
                            Não pode conter caracteres acentuados
                            {montaIcone(validacoes.acentuados)}
                          </Item>
                          <Item
                            status={validacoes.algarismo || validacoes.simbolo}
                          >
                            Um número ou símbolo (caractere especial)
                            {montaIcone(
                              validacoes.algarismo || validacoes.simbolo
                            )}
                          </Item>
                          <Item status={validacoes.tamanho}>
                            Deve ter no mínimo 8 e no máximo 12 caracteres
                            {montaIcone(validacoes.tamanho)}
                          </Item>
                        </Itens>
                      </Validacoes>
                      {validarSeFormularioTemErro() && (
                        <MensagemErro className="rounded p-3 mb-4">
                          Sua nova senha deve conter letras maiúsculas,
                          minúsculas, números e símbolos. Por favor, digite
                          outra senha
                        </MensagemErro>
                      )}
                      {erroGeral && !validarSeFormularioTemErro() && (
                        <MensagemErro className="rounded p-3 mb-4">
                          {erroGeral}
                        </MensagemErro>
                      )}
                      <Div className="mx-auto d-flex justify-content-end">
                        <Button
                          label="Sair"
                          color={Colors.Roxo}
                          border
                          onClick={onClickSair}
                          id="btnSair"
                        />
                        <Button
                          label="Continuar"
                          color={Colors.Roxo}
                          onClick={aoClicarContinuar}
                          id="btnContinuar"
                        />
                      </Div>
                    </Form>
                  </Div>
                </>
              )}
            </Texto>
          </Div>
        </Div>
      </Container>
    </>
  );
};

export default RedefinirSenha;
