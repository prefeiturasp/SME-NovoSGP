import React, { useEffect, useState, useRef, useLayoutEffect } from 'react';
import { useDispatch } from 'react-redux';
import LoginHelper from './helper';
import Row from '~/componentes/row';
import LogoDoSgp from '~/recursos/LogoSgpTexto.svg';
import LogoCidadeSP from '~/recursos/LogoCidadeSP.svg';
import Grid from '~/componentes/grid';
import { Base, Colors } from '~/componentes/colors';
import FormGroup from '~/componentes/formGroup';
import Button from '~/componentes/button';
import {
  Fundo,
  Logo,
  Formulario,
  LogoSGP,
  CampoTexto,
  Rotulo,
  Cartao,
  LogoSP,
  CorpoCartao,
  Centralizar,
  Link,
  LabelLink,
  TextoAjuda,
  ErroTexto,
  ErroGeral,
} from './login.css';
import { Tooltip } from 'antd';
import Icon from '~/componentes/icon';

const Login = props => {
  const errosDefault = { erroGeral: '', erroUsuario: '', erroSenha: '' };

  const dispatch = useDispatch();

  let redirect = null;
  if (props.match && props.match.params && props.match.params.redirect)
    redirect = props.match.params.redirect;

  const helper = new LoginHelper(dispatch, redirect);

  const [login, setLogin] = useState({
    usuario: '',
    senha: '',
    ...errosDefault,
  });

  const usuario = login.usuario;
  const senha = login.senha;

  const campoUsuario = useRef(null);
  const campoSenha = useRef(null);

  useEffect(() => {
    campoUsuario.current.focus();
  }, [usuario]);

  useEffect(() => {
    campoSenha.current.focus();
  }, [senha]);

  const aoPressionarTecla = e => {
    if (e.key === 'Enter') {
      Acessar();
    }
  };

  document.onkeyup = aoPressionarTecla;

  const DefinirUsuario = e => {
    setLogin({ ...login, ...errosDefault, usuario: e.target.value });
  };

  const DefinirSenha = e => {
    setLogin({ ...login, ...errosDefault, senha: e.target.value });
  };

  const Acessar = async () => {
    const { sucesso, ...retorno } = await helper.acessar(login);

    if (!sucesso) {
      setLogin({ ...login, ...retorno });
      return;
    }
  };

  return (
    <Fundo className="p-0">
      <Grid cols={12} className="d-flex justify-content-end">
        <Cartao className="col-xl-6 col-lg-6 col-md-8 col-sm-8 col-xs-12">
          <CorpoCartao className="">
            <Centralizar className="row col-md-12">
              <Row className="col-md-12 p-0 d-flex justify-content-center align-self-start">
                <LogoSGP className="col-xl-8 col-md-8 col-sm-8 col-xs-12">
                  <Logo
                    src={LogoDoSgp}
                    alt="Novo Sistema de Gestão Pedagógica"
                  />
                </LogoSGP>
              </Row>
              <Row className="col-md-12 d-flex justify-content-center align-self-start p-0">
                <Formulario
                  id="Formulario"
                  className="col-xl-8 col-md-8 col-sm-8 col-xs-12 p-0"
                >
                  <FormGroup className="col-md-12 p-0">
                    <Rotulo className="d-block" htmlFor="Usuario">
                      Usuário{' '}
                      <Tooltip placement="top" title={TextoAjuda}>
                        <i className="fas fa-question-circle"></i>
                      </Tooltip>
                    </Rotulo>
                    <CampoTexto
                      id="Usuario"
                      ref={campoUsuario}
                      value={usuario}
                      onChange={DefinirUsuario}
                      placeholder="Insira seu RF ou usuário"
                      className={`col-md-12 form-control ${login.erroSenha &&
                        'is-invalid'}`}
                    />
                    {login.erroUsuario && (
                      <ErroTexto>{login.erroUsuario}</ErroTexto>
                    )}
                  </FormGroup>
                  <FormGroup className="col-md-12 p-0">
                    <Rotulo htmlFor="Senha">Senha</Rotulo>
                    <CampoTexto
                      id="Senha"
                      ref={campoSenha}
                      value={senha}
                      onChange={DefinirSenha}
                      type="password"
                      placeholder="Insira sua senha"
                      className={`col-md-12 form-control ${login.erroSenha &&
                        'is-invalid'}`}
                    />
                    {login.erroSenha && (
                      <ErroTexto>{login.erroSenha}</ErroTexto>
                    )}
                  </FormGroup>
                  <FormGroup>
                    <Button
                      style="primary"
                      className="btn-block d-block"
                      label="Acessar"
                      color={Colors.Roxo}
                      onClick={Acessar}
                    />
                    <Centralizar className="mt-1">
                      <Link to="/recuperar-senha" isactive>
                        <LabelLink>Esqueci minha senha</LabelLink>
                      </Link>
                    </Centralizar>
                  </FormGroup>
                  <FormGroup>
                    {login.erroGeral && (
                      <ErroGeral>{login.erroGeral}</ErroGeral>
                    )}
                  </FormGroup>
                </Formulario>
              </Row>
              <Row className="col-md-12 d-flex justify-content-center align-self-end mb-3">
                <LogoSP className="col-xl-8 col-md-8 col-sm-8 col-xs-12 d-flex">
                  <Logo
                    src={LogoCidadeSP}
                    alt="Cidade de São Paulo - Educação"
                  />
                </LogoSP>
              </Row>
            </Centralizar>
          </CorpoCartao>
        </Cartao>
      </Grid>
    </Fundo>
  );
};

export default Login;
