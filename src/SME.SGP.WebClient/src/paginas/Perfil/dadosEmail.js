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
import api from '~/servicos/api';
import { sucesso, confirmar } from '~/servicos/alertas';
import MensagemAlerta from './mensagemAlerta';

const DadosEmail = () => {

  const [email, setEmail] = useState('teste@teste.com');
  const [emailEdicao, setEmailEdicao] = useState();
  const [senha, setSenha] = useState('******');
  const [visualizarEmail, setVisualizarEmail] = useState(false);
  const [ocultarModalCancelamento, setOcultarModalCancelamento] = useState(true);

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

  const [validacoes] = useState(
    Yup.object({
      emailUsuario: Yup.string()
        .email('Digite um e-mail válido.')
    })
  );

  const salvarEmail = novoEmail => {
    api.put('v1/usuarios/autenticado/email', {novoEmail: novoEmail}).then(resp =>{
      setEmail(novoEmail);
      setVisualizarEmail(false);
      sucesso('Solicitação realizada com sucesso. Verifique sua caixa de entrada');
    })
  }

  const cancelarEdicao = () =>{
    //setVisualizarEmail(false);
    setOcultarModalCancelamento(false);
  }

  return (
    <Campos>
      <Formik
        enableReinitialize
        initialValues={{
          emailUsuario: emailEdicao? emailEdicao: email,
        }}
        validationSchema={validacoes}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={visualizarEmail}
              onConfirmacaoPrincipal={() => {
                form.validateForm().then(() => {
                  form.handleSubmit(e => e);
                  if(form.errors && !form.errors.emailUsuario){
                    salvarEmail(form.values.emailUsuario);
                  }
                });
              }}
              onConfirmacaoSecundaria={() => cancelarEdicao()}
              onClose={() => cancelarEdicao()}
              labelBotaoPrincipal="Confirmar"
              labelBotaoSecundario="Cancelar"
              titulo="Editar E-mail"
              closable={false}
              esconderBotoes={!ocultarModalCancelamento}
            >
              <div hidden={!ocultarModalCancelamento}>
                <CampoTexto
                  label="E-mail"
                  name="emailUsuario"
                  form={form}
                  maxlength="50"
                />
                <div>
                  <AlertaBalao maxWidth={472} marginTop={14} mostrarAlerta={true}
                          texto="Você já possui um endereço de e-mail cadastrado. Ao alterá-lo, todas as comunicações
                          passarão a ser feitas no novo e-mail" />
                </div>
              </div>
              <MensagemAlerta
                oculto={ocultarModalCancelamento}
                confirmar={() => {}}
                cancelar={()=>setVisualizarEmail(false)}/>
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik>

      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado={true}
            value={email}
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
