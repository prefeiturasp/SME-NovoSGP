import React, { useEffect, useState, useRef } from 'react';
import { useDispatch } from 'react-redux';
import LoginHelper from './helper';
import * as Yup from 'yup';
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
import { Formik, Form } from 'formik';
import CampoTexto from '~/componentes/campoTexto';

const Login = props => {

  const [erroGeral, setErroGeral] = useState("");

  const dispatch = useDispatch();

  let redirect = null;

  if (props.match && props.match.params && props.match.params.redirect)
    redirect = props.match.params.redirect;

  const helper = new LoginHelper(dispatch, redirect);

  const [validacoes, setValidacoes] = useState(Yup.object({
    usuario: Yup.string().required("Digite seu Usuário").min(5, "O usuário deve conter no mínimo 5 caracteres."),
    senha: Yup.string().required("Digite sua Senha").min(4, 'A senha deve conter no mínimo 4 caracteres.')
  }));

  const aoPressionarTecla = e => {
    if (e.key === 'Enter') {
      Acessar();
    }
  };
  document.onkeyup = aoPressionarTecla;;

  const Acessar = async (login) => {
    const { sucesso, ...retorno } = await helper.acessar(login);

    if (!sucesso) {
      return;
    }
  };

  const AoClicarBotaoAutenticar = (form, e) => {

    setErroGeral("");

    form.validateForm().then(a => {form.handleSubmit(e); return a;}).catch(() => console.log(e));
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
                  <Formik
                    enableReinitialize
                    initialValues={{ usuario: '', senha: '' }}
                    onSubmit={login => Acessar(login)}
                    validationSchema={validacoes}
                    validateOnBlur={false}
                    validateOnChange={false}
                  >
                    {form => (
                      <Formulario>
                        <Rotulo className="d-block" htmlFor="usuario">
                          Usuário{' '}
                          <Tooltip placement="top" title={TextoAjuda}>
                            <i className="fas fa-question-circle"></i>
                          </Tooltip>
                        </Rotulo>
                        <CampoTexto
                          form={form}
                          name="usuario"
                          classNameCampo="mb-3"
                          id="usuario"
                          maxlength={50}
                          placeholder="Informe o RF ou usuário"
                          type="input"
                          icon />
                        <Rotulo htmlFor="Senha">Senha</Rotulo>
                        <CampoTexto
                          form={form}
                          name="senha"
                          id="senha"
                          classNameCampo="mb-3"
                          maxlength={50}
                          placeholder="Informe sua senha"
                          type="input"
                          icon />
                        <FormGroup>
                          <Button
                            style="primary"
                            className="btn-block d-block"
                            label="Acessar"
                            color={Colors.Roxo}
                            onClick={e => AoClicarBotaoAutenticar(form, e)}
                          />
                          <Centralizar className="mt-1">
                            <Link to="/" isactive>
                              <LabelLink>Esqueci minha senha</LabelLink>
                            </Link>
                          </Centralizar>
                        </FormGroup>
                        <FormGroup>
                          {erroGeral && <ErroGeral>{erroGeral}</ErroGeral>}                          
                        </FormGroup>
                      </Formulario>
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
