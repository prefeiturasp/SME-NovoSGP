import React, { useState, useEffect } from 'react';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import AlertaBalao from '~/componentes/alertaBalao';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import api from '~/servicos/api';
import { sucesso, confirmar } from '~/servicos/alertas';
import { useSelector } from 'react-redux';
import { store } from '~/redux';
import { meusDadosSalvarEmail } from '~/redux/modulos/usuario/actions';
import styled from 'styled-components';
import FormularioSenha from './FormularioSenha/formularioSenha';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { Redirect } from 'react-router-dom';

const Campos = styled.div`
  margin-right: 10px;
  margin-left: 40px;
  .campo {
    margin-top: 50px;
  }

  .botao {
    margin-top: 25px;
  }
`;

const DadosEmail = () => {
  const usuarioStore = useSelector(state => state.usuario);
  const [email, setEmail] = useState(usuarioStore.meusDados.email);
  const [emailEdicao, setEmailEdicao] = useState(usuarioStore.meusDados.email);
  const [visualizarFormEmail, setVisualizarFormEmail] = useState(false);
  const [erroEmail, setErroEmail] = useState('');

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const permissoesTela = usuarioStore.permissoes[RotasDto.MEUS_DADOS];

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const [validacoes] = useState(
    Yup.object({
      emailUsuario: Yup.string()
        .nullable()
        .required('E-mail obrigatório')
        .email('Digite um e-mail válido.')
        .test({
          name: 'ehSme',
          exclusive: true,
          message: "O e-mail deve ser do domínio '@sme.prefeitura.sp.gov.br'",
          test: value =>
            usuarioStore.possuiPerfilSmeOuDre
              ? value.includes('@sme.prefeitura.sp.gov.br')
              : true,
        }),
    })
  );

  const salvarEmail = novoEmail => {
    api
      .put('v1/usuarios/autenticado/email', {
        novoEmail: novoEmail.emailUsuario,
      })
      .then(resp => {
        setEmail(novoEmail.emailUsuario);
        setEmailEdicao('');
        setErroEmail('');
        setVisualizarFormEmail(false);
        store.dispatch(meusDadosSalvarEmail(novoEmail.emailUsuario));
        sucesso(
          'Solicitação realizada com sucesso. Verifique sua caixa de entrada'
        );
      })
      .catch(err => {
        setEmailEdicao(novoEmail.emailUsuario);

        if (!err.response) {
          setErroEmail('Não foi possivel se comunicar com o servidor');
          return;
        }

        if (!err.response.data) {
          setErroEmail('Ocorreu um erro, por favor contate o suporte');
          return;
        }

        const { mensagens } = err.response.data;

        if (mensagens) setErroEmail(mensagens.join(','));
      });
  };

  const onClickCancelar = async form => {
    const novoEmail = form.values.emailUsuario;
    if (email !== novoEmail && !form.errors.emailUsuario) {
      setVisualizarFormEmail(false);
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        setEmailEdicao(novoEmail);
        form.validateForm().then(err => {
          form.handleSubmit(e => e);
        });
      } else {
        setVisualizarFormEmail(false);
        setErroEmail('');
        setEmailEdicao(email);
      }
    } else {
      setVisualizarFormEmail(false);
      setEmailEdicao(email);
      setErroEmail('');
    }
  };

  return (
    <Campos>
      <Formik
        initialValues={{
          emailUsuario: emailEdicao || email,
        }}
        validationSchema={validacoes}
        onSubmit={valor => salvarEmail(valor)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={visualizarFormEmail}
              onConfirmacaoPrincipal={() => {
                form.validateForm().then(() => {
                  form.handleSubmit(e => e);
                });
              }}
              onConfirmacaoSecundaria={() => onClickCancelar(form)}
              onClose={() => onClickCancelar(form)}
              labelBotaoPrincipal="Confirmar"
              labelBotaoSecundario="Cancelar"
              titulo="Editar E-mail"
              closable
            >
              <div>
                <CampoTexto
                  label="E-mail"
                  name="emailUsuario"
                  form={form}
                  maxlength="50"
                />
                <div className={`${(!email || erroEmail !== '') && 'd-none'}`}>
                  <AlertaBalao
                    maxWidth={472}
                    marginTop={14}
                    mostrarAlerta
                    texto="Você já possui um endereço de e-mail cadastrado. Ao alterá-lo, todas as comunicações
                          passarão a ser feitas no novo e-mail"
                  />
                </div>
                <div className={`${erroEmail === '' && 'd-none'}`}>
                  <AlertaBalao
                    maxWidth={472}
                    marginTop={14}
                    mostrarAlerta
                    texto={erroEmail}
                  />
                </div>
              </div>
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik>

      <div className="row campo w-100">
        <div className="col-md-10">
          <CampoTexto
            desabilitado
            value={email}
            label="E-mail"
            placeholder="Clique em editar para inserir um e-mail"
            onChange={() => {}}
            type="email"
          />
        </div>
        <div className="col-md-2 botao">
          <Button
            label="Editar"
            color={Colors.Roxo}
            disabled={
              !permissoesTela || somenteConsulta || !permissoesTela.podeAlterar
            }
            border
            bold
            onClick={() => setVisualizarFormEmail(true)}
          />
        </div>
      </div>
      <FormularioSenha />
    </Campos>
  );
};

export default DadosEmail;
