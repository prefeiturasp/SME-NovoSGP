import React, { useState } from 'react';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import styled from 'styled-components';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import AlertaBalao from '~/componentes/alertaBalao';
import { Formik } from 'formik';
import * as Yup from 'yup';
import { Form } from 'antd';

const DadosEmail = () => {

  const [email, setEmail] = useState('teste@teste.com');
  const [senha, setSenha] = useState('123456');
  const [visualizarEmail, setVisualizarEmail] = useState(false);

  const Campos = styled.div`
    margin-right: 10px;
    margin-left: 40px;
    .campo{
      margin-top: 50px;
    }

    .botao{
      margin-top: 25px;
    }
  `;

  const onChangeEmail = campoEmail => {
    setEmail(campoEmail.target.value);
  };

  // const [refForm, setRefForm] = useState();

  // const [validacoes] = useState(
  //   Yup.object({
  //     emailUsuario: Yup.string()
  //       .email('Digite um e-mail válido.')
  //   })
  // );

  return (
    <Campos>

      {/* <Formik
        ref={refFormik => setRefForm(refFormik)}
        enableReinitialize
        initialValues={{
          emailUsuario: email,
        }}
        validationSchema={validacoes}
        onSubmit={() => {}}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={true}
              onConfirmacaoPrincipal={() => {
                form.validateForm().then(() => form.handleSubmit(e => e));
              }}
              onConfirmacaoSecundaria={() => setVisualizarEmail(false)}
              onClose={() => setVisualizarEmail(false)}
              labelBotaoPrincipal="Confirmar"
              labelBotaoSecundario="Cancelar"
              titulo="Editar E-mail"
              closable={false}
            >
              <CampoTexto
                label="E-mail"
                name="emailUsuario"
                form={form}
                maxlength="50"
              />
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik> */}

      <ModalConteudoHtml
        key={'editarEmail'}
        visivel={visualizarEmail}
        onConfirmacaoPrincipal={() => { }}
        onConfirmacaoSecundaria={() => setVisualizarEmail(false)}
        onClose={() => { }}
        labelBotaoPrincipal="Confirmar"
        labelBotaoSecundario="Cancelar"
        desabilitarBotaoPrincipal={false}
        titulo="Editar E-mail"
        closable={true}
        onClose={() => setVisualizarEmail(false)}
      >
        <CampoTexto
          desabilitado={false}
          label="E-mail"
          value={email}
          placeholder="Insira um e-mail"
          onChange={(e) => onChangeEmail(e)}
          type="email"
        />
        <div>
        <AlertaBalao maxWidth={472} marginTop={14} mostrarAlerta={true}
                texto="Você já possui um endereço de e-mail cadastrado. Ao alterá-lo, todas as comunicações
                passarão a ser feitas no novo e-mail" />
        </div>
      </ModalConteudoHtml>

      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado={true}
            label="E-mail"
            placeholder="Insira um e-mail"
            onChange={() => { }}
            type="email"
          />
        </div>
        <div className="col-md-2 botao">
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            onClick={() => setVisualizarEmail(true)}
          />
        </div>
      </div>
      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado={true}
            label="Senha"
            value={senha}
            className="col-11 campo"
            placeholder="Insira uma senha"
            onChange={() => { }}
            type="password"
          />
        </div>
        <div className="col-md-2 botao">
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            onClick={() => { }}
          />
        </div>
      </div>
    </Campos>
  );
}

export default DadosEmail;
