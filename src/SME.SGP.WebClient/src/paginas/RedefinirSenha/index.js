import React, { useState, useRef, useLayoutEffect, useEffect } from 'react';
import styled from 'styled-components';
import { Formik, Form } from 'formik';
import LogoDoSgp from '~/recursos/LogoDoSgp.svg';
import { Base, Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import Servico from '~/servicos/Paginas/RedefinirSenhaService';
import { Nav, Logo, Div, Container, Texto, Titulo, Rotulo, CampoTexto, Validacoes, Itens, Icone, MensagemErro } from './index.css';
import { URL_LOGIN } from '~/constantes/url';

const RedefinirSenha = (props) => {

  const [dados, setDados] = useState({
    senha: '',
    confirmarSenha: '',
  });

  const [tokenValidado, setTokenValidado] = useState(false);

  const senha = dados.senha;
  const confirmarSenha = dados.confirmarSenha;

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

  const Item = styled.li`
    ${props => props.status === true && `color: ${Base.Verde}`};
    ${props => props.status === false && `color: ${Base.VermelhoNotificacao}`};
    font-weight: normal;
  `;

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

  useEffect(() => {
    inputSenhaRef.current.focus();
  }, [senha]);

  useEffect(() => {
    if (!tokenValidado)
      validarToken();
  });

  useEffect(() => {
    inputConfSenhaRef.current.focus();
  }, [confirmarSenha]);

  const AoMudarSenha = () => {
    setDados({ ...dados, senha: inputSenhaRef.current.value });

    realizarValidacoes(inputSenhaRef.current.value);
  };

  const validarToken = async () => {

    const token = props.match && props.match.params && props.match.params.token;

    if (!token)
      history.push(URL_LOGIN);

    const tokenValido = await Servico.validarToken(token);

    if (!tokenValido)
      history.push(URL_LOGIN);
    else
      setTokenValidado(true);
  };

  const AoMudarConfSenha = () => {
    setDados({ ...dados, confirmarSenha: inputConfSenhaRef.current.value });

    realizarValidacoes(inputConfSenhaRef.current.value);
  };

  const realizarValidacoes = (valor) => {
    const temMaiuscula = valor.match(/([A-Z])/);
    const temMinuscula = valor.match(/([a-z])/);
    const temAlgarismo = valor.match(/([0-9])/);
    const temSimbolo = valor.match(/([!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?])/);
    const temAcento = valor.match(/([À-ÖØ-öø-ÿ])/);
    const espacoBranco = valor.includes(" ");

    const iguais = inputConfSenhaRef.current.value === inputSenhaRef.current.value;

    let tamanho = valor.length >= 8 && valor.length <= 12;

    setValidacoes({
      maiuscula: !!temMaiuscula,
      minuscula: !!temMinuscula,
      algarismo: !!temAlgarismo || !!temSimbolo,
      simbolo: !!temSimbolo || !!temAlgarismo,
      acentuados: !temAcento,
      espacoBranco: !espacoBranco,
      tamanho: !!tamanho,
      iguais: !!iguais
    });
  }

  const validarSeFormularioTemErro = () => Object.entries(validacoes).filter(validar => !validar[1]).length > 0;

  const onClickSair = () => {
    history.push('/');
  };

  const alterarSenha = () => {
    Servico.redefinirSenha();
  };

  const aoClicarContinuar = () => {
    realizarValidacoes(inputSenhaRef.current.value);

    if (!validarSeFormularioTemErro())
      alterarSenha();
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
              <Titulo style={{ marginTop: '70px', marginBottom: '40px' }}>
                Nova Senha
              </Titulo>
              <Div style={{ marginBottom: '60px' }}>
                Identificamos que você ainda não definiu uma senha pessoal para
                acesso ao SGP. Este passo é obrigatório para que você tenha
                acesso ao sistema.
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
                      onChange={AoMudarSenha}
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
                      onChange={AoMudarConfSenha}
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
                      <Item status={validacoes.algarismo || validacoes.simbolo}>
                        Um número ou símbolo (caractere especial)
                            {montaIcone(validacoes.algarismo || validacoes.simbolo)}
                      </Item>
                      <Item status={validacoes.tamanho}>
                        Deve ter no mínimo 8 e no máximo 12 caracteres
                            {montaIcone(validacoes.tamanho)}
                      </Item>
                    </Itens>
                  </Validacoes>
                  {validarSeFormularioTemErro() && (
                    <MensagemErro className="rounded p-3 mb-3">
                      Sua nova senha deve conter letras maiúsculas,
                      minúsculas, números e símbolos. Por favor, digite
                      outra senha
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
            </Texto>
          </Div>
        </Div>
      </Container>
    </>
  );
};

export default RedefinirSenha;
