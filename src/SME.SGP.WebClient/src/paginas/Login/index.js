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
  const errosDefault = { erroGeral: '', erroUsuario: '', erroSenha: '' };

  const dispatch = useDispatch();

  let redirect = null;

  if (props.match && props.match.params && props.match.params.redirect)
    redirect = props.match.params.redirect;

  const helper = new LoginHelper(dispatch, redirect);

  const [login, setLogin] = useState({
    ...errosDefault,
  });

  const [validacoes, setValidacoes] = useState(Yup.object({
    usuario: Yup.string().required().min(5, "O usuário deve conter no mínimo 5 caracteres."),
    senha: Yup.string().required().min(4,'A senha deve conter no mínimo 4 caracteres.')
  }));

  const aoPressionarTecla = e => {
    if (e.key === 'Enter') {
      Acessar();
    }
  };
  document.onkeyup = aoPressionarTecla;;

  const Acessar = async () => {
    const { sucesso, ...retorno } = await helper.acessar(login);

    if (!sucesso) {
      setLogin({ ...login, ...retorno });
      return;
    }
  };

  const onSubmit = (obj) => console.log(obj);

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
                  enableReinitialize={true} 
                  initialValues={{usuario: '', login: ''}} 
                  onSubmit={onSubmit} 
                  validationSchema={validacoes} 
                  validateOnBlur 
                  validateOnChange
                  >
                    {form => (
                      <Form>
                        <CampoTexto form={form} maxlength={50} placeholder="Informe o RF ou usuário"></CampoTexto>
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
