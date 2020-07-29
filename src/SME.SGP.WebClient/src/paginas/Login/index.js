import React, { useState, useRef, useEffect } from 'react';
import PropTypes from 'prop-types';

import { useDispatch, useSelector } from 'react-redux';
import * as Yup from 'yup';
import { Tooltip } from 'antd';
import { Formik, Form } from 'formik';
import shortid from 'shortid';
import { BrowserView, MobileView, isBrowser } from 'react-device-detect';
import LoginHelper from './loginHelper';
import Row from '~/componentes/row';
import LogoDoSgp from '~/recursos/LogoSgpTexto.svg';
import LogoCidadeSP from '~/recursos/LogoCidadeSP.svg';
import Grid from '~/componentes/grid';
import { Colors } from '~/componentes/colors';
import FormGroup from '~/componentes/formGroup';
import Button from '~/componentes/button';
import {
  Fundo,
  Logo,
  Formulario,
  LogoSGP,
  Rotulo,
  Cartao,
  LogoSP,
  CorpoCartao,
  Centralizar,
  LabelLink,
  TextoAjuda,
  ErroGeral,
  MensagemMobile,
} from './login.css';
import CampoTexto from '~/componentes/campoTexto';
import { URL_RECUPERARSENHA } from '~/constantes/url';
import history from '~/servicos/history';
import { Loader } from '~/componentes';
import { setExibirMensagemSessaoExpirou } from '~/redux/modulos/mensagens/actions';

const Login = props => {
  const dispatch = useDispatch();
  const inputUsuarioRf = useRef();
  const btnAcessar = useRef();

  const [carregando, setCarregando] = useState(false);

  const [erroGeral, setErroGeral] = useState('');
  const [login, setLogin] = useState({
    usuario: '',
    senha: '',
  });

  const exibirMensagemSessaoExpirou = useSelector(
    store => store.usuario.sessaoExpirou
  );

  const { versao } = useSelector(store => store.sistema);

  const { match } = props;
  const redirect = match?.params?.redirect ? match.params.redirect : null;

  const helper = new LoginHelper(dispatch, redirect);

  const [validacoes] = useState(
    Yup.object({
      usuario: Yup.string()
        .required('Digite seu Usuário')
        .min(5, 'O usuário deve conter no mínimo 5 caracteres.'),
      senha: Yup.string()
        .required('Digite sua Senha')
        .min(4, 'A senha deve conter no mínimo 4 caracteres.'),
    })
  );

  const realizarLogin = async dados => {
    setCarregando(true);

    setLogin({
      usuario: dados.usuario,
      senha: dados.senha,
    });

    setErroGeral('');
    dispatch(setExibirMensagemSessaoExpirou(false));

    const { sucesso, ...retorno } = await helper.acessar(dados);

    if (!sucesso) {
      setErroGeral(retorno.erroGeral);
      setCarregando(false);
    }
  };

  const aoPressionarTecla = e => {
    if (e.key === 'Enter') {
      e.preventDefault();
      btnAcessar.current.click();
    }
  };

  const aoClicarBotaoAutenticar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
  };

  useEffect(() => {
    document.addEventListener('keyup', aoPressionarTecla);
    return () => {
      document.removeEventListener('keyup', aoPressionarTecla);
    };
  }, []);

  useEffect(() => {
    if (exibirMensagemSessaoExpirou) {
      setErroGeral('Sua sessão expirou!');
    }
  }, [exibirMensagemSessaoExpirou]);

  const navegarParaRecuperarSenha = () => {
    const rf =
      inputUsuarioRf && inputUsuarioRf.currrent && inputUsuarioRf.current.value;
    history.push({
      pathname: URL_RECUPERARSENHA,
      state: {
        rf,
      },
    });
  };

  return (
    <Fundo className="p-0 h-100 overflow-hidden">
      <Grid cols={12} className="d-flex justify-content-end overflow-hidden">
        <Cartao className="col-xl-6 col-lg-6 col-md-8 pt-1 pb-0 col-sm-8 col-xs-12 overflow-hidden">
          <CorpoCartao className=" overflow-hidden">
            <Centralizar className="row col-md-12 overflow-hidden">
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
                  {isBrowser ? (
                    <Formik
                      enableReinitialize
                      initialValues={{
                        usuario: login.usuario,
                        senha: login.senha,
                      }}
                      onSubmit={dados => realizarLogin(dados)}
                      validationSchema={validacoes}
                      validateOnBlur={false}
                      validateOnChange={false}
                    >
                      {form => (
                        <Form>
                          <Rotulo className="d-block" htmlFor="usuario">
                            Usuário
                            <Tooltip placement="top" title={TextoAjuda}>
                              <i className="fas fa-question-circle ml-1" />
                            </Tooltip>
                          </Rotulo>
                          <CampoTexto
                            form={form}
                            name="usuario"
                            id="usuario"
                            maxlength={50}
                            classNameCampo="mb-3"
                            placeholder="Informe o RF ou usuário"
                            type="input"
                            ref={inputUsuarioRf}
                            icon
                          />
                          <Rotulo htmlFor="Senha">Senha</Rotulo>
                          <CampoTexto
                            form={form}
                            name="senha"
                            id="senha"
                            maxlength={50}
                            classNameCampo="mb-3"
                            placeholder="Informe sua senha"
                            type="input"
                            maskType="password"
                            icon
                          />
                          <FormGroup>
                            <Loader loading={carregando} tip="">
                              <Button
                                id={shortid.generate()}
                                className="btn-block d-block"
                                label="Acessar"
                                color={Colors.Roxo}
                                ref={btnAcessar}
                                onClick={e => aoClicarBotaoAutenticar(form, e)}
                              />
                            </Loader>
                            <Centralizar className="mt-1">
                              <LabelLink onClick={navegarParaRecuperarSenha}>
                                Esqueci minha senha
                              </LabelLink>
                            </Centralizar>
                          </FormGroup>
                          {form.errors.usuario || form.errors.senha ? (
                            <ErroGeral>
                              Você precisa informar um usuário e senha para
                              acessar o sistema.
                            </ErroGeral>
                          ) : null}
                          {erroGeral &&
                          !(form.errors.usuario || form.errors.senha) ? (
                            <ErroGeral>{erroGeral}</ErroGeral>
                          ) : null}
                        </Form>
                      )}
                    </Formik>
                  ) : (
                    <MensagemMobile>
                      <span>
                        Para sua melhor experiência recomendamos que o acesso ao
                        sistema seja realizado pelo computador.
                      </span>
                    </MensagemMobile>
                  )}
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
              <Row>
                {!versao ? '' : <strong>{versao}&nbsp;</strong>} - Sistema
                homologado para navegadores: Google Chrome e Firefox
              </Row>
            </Centralizar>
          </CorpoCartao>
        </Cartao>
      </Grid>
    </Fundo>
  );
};

Login.propTypes = {
  match: PropTypes.oneOfType([PropTypes.any]),
};

Login.defaultProps = {
  match: {},
};

export default Login;
