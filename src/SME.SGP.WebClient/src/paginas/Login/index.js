import React, { useState, useRef } from 'react';
import { useDispatch } from 'react-redux';
import * as Yup from 'yup';
import { Tooltip } from 'antd';
import { Formik, Form } from 'formik';
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
  Link,
  LabelLink,
  TextoAjuda,
  ErroGeral,
} from './login.css';
import CampoTexto from '~/componentes/campoTexto';
import { URL_RECUPERARSENHA } from '~/constantes/url';

const Login = props => {
  const dispatch = useDispatch();
  const btnAcessar = useRef();

  const [erroGeral, setErroGeral] = useState('');
  const [login, setLogin] = useState({
    usuario: '',
    senha: '',
  });

  let redirect = null;

  if (props.match && props.match.params && props.match.params.redirect)
    redirect = props.match.params.redirect;

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

  const aoPressionarTecla = e => {
    if (e.key === 'Enter') {
      e.preventDefault();
      btnAcessar.current.click();
    }
  };
  document.onkeyup = aoPressionarTecla;

  const Acessar = async dados => {
    console.log('acessar');
    setLogin({
      usuario: dados.usuario,
      senha: dados.senha,
    });

    setErroGeral('');

    const { sucesso, ...retorno } = await helper.acessar(dados);

    if (!sucesso) setErroGeral(retorno.erroGeral);
  };

  const aoClicarBotaoAutenticar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
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
                  <Formik
                    enableReinitialize
                    initialValues={{
                      usuario: login.usuario,
                      senha: login.senha,
                    }}
                    onSubmit={dados => Acessar(dados)}
                    validationSchema={validacoes}
                    validateOnBlur={false}
                    validateOnChange={false}
                  >
                    {form => (
                      <Form>
                        <Rotulo className="d-block" htmlFor="usuario">
                          Usuário
                          <Tooltip placement="top" title={TextoAjuda}>
                            <i className="fas fa-question-circle" />
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
                          <Button
                            className="btn-block d-block"
                            label="Acessar"
                            color={Colors.Roxo}
                            ref={btnAcessar}
                            onClick={e => aoClicarBotaoAutenticar(form, e)}
                          />
                          <Centralizar className="mt-1">
                            <Link to={URL_RECUPERARSENHA} isactive>
                              <LabelLink>Esqueci minha senha</LabelLink>
                            </Link>
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
