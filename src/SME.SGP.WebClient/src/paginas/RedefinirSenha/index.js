import React, { useState, useRef, useLayoutEffect } from 'react';
import styled from 'styled-components';
import { Formik, Form } from 'formik';
import LogoDoSgp from '~/recursos/LogoDoSgp.svg';
import { Base, Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import api from '~/servicos/api';

const RedefinirSenha = () => {
  const [senha, setSenha] = useState('');
  const [validacoes, setValidacoes] = useState({
    maiuscula: '',
    minuscula: '',
    algarismo: '',
    simbolo: '',
    acentos: '',
    tamanho: '',
  });

  const Nav = styled.nav`
    height: 70px;
  `;

  const Logo = styled.img`
    max-height: 65px;
    max-width: 75px;
  `;

  const Div = styled.div`
    button {
      margin-right: 1rem;
    }
    button:last-child {
      margin-right: 0;
    }
  `;

  const Container = styled(Div)`
    background: ${Base.Branco};
    height: 100%;
  `;

  const Texto = styled(Div)`
    font-size: 14px;
    letter-spacing: normal;
    line-height: normal;
  `;

  const Titulo = styled.h1`
    font-size: 24px;
    font-weight: bold;
  `;

  const Rotulo = styled.label`
    color: ${Base.CinzaMako};
  `;

  const CampoTexto = styled.input`
    color: ${Base.CinzaBotao};
    font-size: 14px;
  `;

  const Validacoes = styled(Div)`
    color: ${Base.CinzaBotao};
    font-size: 12px;
    font-weight: bold;
  `;

  const Itens = styled.ul`
    line-height: 18px;
  `;

  const Item = styled.li`
    ${props => props.status === true && `color: ${Base.Verde}`};
    ${props => props.status === false && `color: ${Base.VermelhoNotificacao}`};
    font-weight: normal;
  `;

  const Icone = styled.i`
    font-size: 16px;
    font-style: normal;
    line-height: 18px;
    margin-left: 5px;
  `;

  const montaIcone = status => {
    let estilo = 'd-none';
    if (typeof status === 'boolean') {
      if (status) estilo = 'd-inline-block fa-check';
      else estilo = 'd-inline-block fa-times';
    }
    return <Icone className={estilo} />;
  };

  const MensagemErro = styled(Div)`
    border: solid 2px ${Base.VermelhoNotificacao};
    color: ${Base.VermelhoNotificacao};
    max-height: 85px;
    max-width: 295px;
  `;

  const inputSenhaRef = useRef();

  useLayoutEffect(() => {
    inputSenhaRef.current.focus();
  }, [senha]);

  const onChangeSenha = () => {
    setSenha(inputSenhaRef.current.value);
    const temMaiuscula = inputSenhaRef.current.value.match(/([A-Z])/);
    const temMinuscula = inputSenhaRef.current.value.match(/([a-z])/);
    const temAlgarismo = inputSenhaRef.current.value.match(/([0-9])/);
    const temSimbolo = inputSenhaRef.current.value.match(
      /([!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?])/
    );
    const temAcento = inputSenhaRef.current.value.match(/([A-Za-zÀ-ÖØ-öø-ÿ])/);

    let tamanho = false;
    if (
      inputSenhaRef.current.value.length >= 8 &&
      inputSenhaRef.current.value.length <= 12
    ) {
      tamanho = true;
    }

    setValidacoes({
      maiuscula: !!temMaiuscula,
      minuscula: !!temMinuscula,
      algarismo: !!temAlgarismo,
      simbolo: !!temSimbolo,
      acentuados: !temAcento,
      tamanho: !!tamanho,
    });
  };

  const alterarSenha = () => {
    api
      .post('/v1/autenticacao/TROCAR PARA RECUPERAR', {
        usuario: 'string',
        rfcpf: 'string',
        usuarioExterno: 'boolean',
        novaSenha: 'string',
        confirmarSenha: 'string',
      })
      .then(res => {
        if (res.data) {
          return res.data;
        }
        return false;
      });
  };

  const onClickSair = () => {
    history.push('/');
  };

  const onClickContinuar = (form, e) => {
    form.validateForm().then(() => form.handleSubmit(e));
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
                <Formik enableReinitialize>
                  {form => (
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
                          onChange={onChangeSenha}
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
                          <Item status={validacoes.algarismo}>
                            Um algarismo (número)
                            {montaIcone(validacoes.algarismo)}
                          </Item>
                          <Item status={validacoes.simbolo}>
                            Um símbolo (caractere especial)
                            {montaIcone(validacoes.simbolo)}
                          </Item>
                          <Item status={validacoes.acentuados}>
                            Não pode permitir caracteres acentuados
                            {montaIcone(validacoes.acentuados)}
                          </Item>
                          <Item status={validacoes.tamanho}>
                            Deve ter no mínimo 8 e no máximo 12 caracteres
                            {montaIcone(validacoes.tamanho)}
                          </Item>
                        </Itens>
                      </Validacoes>
                      {Object.entries(validacoes).filter(validar => !validar[1])
                        .length > 0 && (
                        <MensagemErro className="rounded p-3">
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
                          onClick={e => onClickContinuar(form, e)}
                          id="btnContinuar"
                        />
                      </Div>
                    </Form>
                  )}
                </Formik>
              </Div>
            </Texto>
          </Div>
        </Div>
      </Container>
    </>
  );
};

export default RedefinirSenha;
