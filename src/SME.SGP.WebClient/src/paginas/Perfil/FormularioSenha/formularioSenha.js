/* eslint-disable func-names */
import React, { useState, useEffect } from 'react';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import '~/servicos/Validacoes/regex';
import CampoTexto from '~/componentes/campoTexto';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import { Validacoes, Validacao } from './formularioSenha.css';
import api from '~/servicos/api';
import { sucesso } from '~/servicos/alertas';
import AlertaBalao from '~/componentes/alertaBalao';
import { useSelector } from 'react-redux';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const FormularioSenha = () => {
  const [exibirModal, setExibirModal] = useState(false);
  const [erroAlertaBalao, setErroAlertaBalao] = useState('');
  const [exibirAlertaBalao, setExibirAlertaBalao] = useState(false);
  const usuarioStore = useSelector(store => store.usuario);
  const permissoesTela = usuarioStore.permissoes[RotasDto.MEUS_DADOS];
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  const fecharModal = form => {
    form.handleReset();
    setExibirModal(false);
    setErroAlertaBalao('');
    setExibirAlertaBalao(false);
  };

  const [validacoes] = useState(
    Yup.object({
      senhaAtual: Yup.string().required('A senha atual deve ser informada'),
      novaSenha: Yup.mixed()
        .contem(/([A-Z])/, 'maiuscula')
        .contem(/([a-z])/, 'minuscula')
        .naoContem(/([À-ÖØ-öø-ÿ])/, 'acentos')
        .naoContem(' ', 'espaco')
        .test('tamanho', 'tamanho', function(valor) {
          return valor && (valor.length >= 8 && valor.length <= 12);
        })
        .test('simbolos', 'simbolos', function(valor) {
          return (
            valor &&
            (valor.match(/([!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?])/) ||
              valor.match(/([0-9])/))
          );
        })
        .test('iguais', 'iguais', function(confirmacao) {
          return confirmacao === this.options.parent.confirmacaoNovaSenha;
        }),
      confirmacaoNovaSenha: Yup.mixed().test(
        'match',
        'As senhas devem ser iguais',
        function(confirmacao) {
          return confirmacao && confirmacao === this.options.parent.novaSenha;
        }
      ),
    })
  );

  const possuiErroCampo = (form, nomeCampo, regra) => {
    const campoPreenchido = Boolean(form.touched[nomeCampo]);

    const mensagem = {
      exibir: form.touched[nomeCampo],
      status: campoPreenchido ? form.errors[nomeCampo] !== regra : false,
    };

    return mensagem;
  };

  const montaIcone = mensagem => {
    let estilo = 'd-none';
    if (!mensagem.exibir) {
      estilo = 'd-none';
    } else if (mensagem.status) estilo = 'd-inline-block fa fa-check';
    else estilo = 'd-inline-block fa fa-times';
    return <i className={estilo} />;
  };

  const validacao = (exibirMensagem, mensagem) => {
    return (
      <Validacao mensagem={exibirMensagem}>
        {mensagem}
        {montaIcone(exibirMensagem)}
      </Validacao>
    );
  };

  const alterarSenha = form => {
    setErroAlertaBalao('');
    setExibirAlertaBalao(false);
    api
      .put('v1/autenticacao/senha', form.values)
      .then(() => {
        sucesso('Senha alterada com sucesso.');
        fecharModal(form);
      })
      .catch(listaErros => {
        if (listaErros && listaErros.response && listaErros.response.data) {
          const mensagens = listaErros.response.data.mensagens.join('<br>');
          setErroAlertaBalao(mensagens);
          setExibirAlertaBalao(true);
        }
      });
  };

  return (
    <div className="row campo w-100">
      <Formik
        enableReinitialize
        initialValues={{
          novaSenha: '',
          senhaAtual: '',
          confirmacaoNovaSenha: '',
        }}
        validationSchema={validacoes}
        onSubmit={form => alterarSenha(form)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={exibirModal}
              onConfirmacaoPrincipal={() => {
                form.validateForm().then(() => {
                  if (form.isValid) {
                    alterarSenha(form);
                  }
                });
              }}
              onConfirmacaoSecundaria={() => fecharModal(form)}
              onClose={() => fecharModal(form)}
              labelBotaoPrincipal="Confirmar"
              labelBotaoSecundario="Cancelar"
              titulo="Nova Senha"
              closable
            >
              <div className="row">
                <div className="col-xs-7 col-md-7 col-lg-7">
                  <div className="row">
                    <div className="col-xs-12 col-md-12 col-lg-12">
                      <CampoTexto
                        label="Senha atual"
                        name="senhaAtual"
                        form={form}
                        maskType="password"
                        maxlength="50"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-xs-7 col-md-7 col-lg-7">
                  <div className="row pt-4">
                    <div className="col-xs-12 col-md-12 col-lg-12">
                      <CampoTexto
                        label="Nova Senha"
                        name="novaSenha"
                        form={form}
                        maxlength="12"
                        maskType="password"
                        semMensagem
                      />
                    </div>
                  </div>
                  <div className="row pt-4">
                    <div className="col-xs-12 col-md-12 col-lg-12">
                      <CampoTexto
                        label="Confirmação da Nova Senha"
                        name="confirmacaoNovaSenha"
                        form={form}
                        maskType="password"
                        maxlength="12"
                      />
                    </div>
                  </div>
                </div>
                <div className="col-xs-5 col-md-5 col-lg-4 ml-4">
                  <div className="row">
                    <Validacoes
                      className="text-left"
                      style={{ marginBottom: '30px' }}
                    >
                      <div style={{ lineHeight: '1.8' }}>
                        Requisitos de segurança da senha:
                      </div>
                      <ul className="validacoes list-unstyled mt-1">
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'maiuscula'),
                          'Uma letra maiúscula'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'minuscula'),
                          'Uma letra minúscula'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'iguais'),
                          'As senhas devem ser iguais'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'espaco'),
                          'Não pode conter espaços em branco'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'acentos'),
                          'Não pode conter caracteres acentuados'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'simbolos'),
                          'Um número ou símbolo (caractere especial)'
                        )}
                        {validacao(
                          possuiErroCampo(form, 'novaSenha', 'tamanho'),
                          'Deve ter no mínimo 8 e no máximo 12 caracteres'
                        )}
                      </ul>
                    </Validacoes>
                  </div>
                </div>
              </div>
              <AlertaBalao
                maxWidth={472}
                mostrarAlerta={exibirAlertaBalao}
                texto={erroAlertaBalao}
              />
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik>
      <div className="col-md-10">
        <CampoTexto
          desabilitado
          label="Senha"
          className="col-11 campo"
          placeholder="************"
          onChange={() => {}}
          type="password"
        />
      </div>
      <div className="col-md-2 botao">
        <Button
          label="Editar"
          color={Colors.Roxo}
          disabled={
            somenteConsulta || !permissoesTela || !permissoesTela.podeAlterar
          }
          border
          bold
          onClick={() => setExibirModal(true)}
        />
      </div>
    </div>
  );
};
export default FormularioSenha;
