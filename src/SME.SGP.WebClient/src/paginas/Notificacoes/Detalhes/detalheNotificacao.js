import React, { useEffect, useState } from 'react';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import { erros, erro, sucesso, confirmar } from '~/servicos/alertas';
import Card from '~/componentes/card';
import { EstiloDetalhe } from './detalheNotificacao.css';
import api from '~/servicos/api';
import * as cores from '~/componentes/colors';
import Button from '~/componentes/button';
import Cabecalho from '~/componentes-sgp/cabecalho';
import CampoTexto from '~/componentes/campoTexto';
import EstiloLinhaTempo from './linhaTempo.css';
import LinhaTempo from '~/componentes/linhaTempo/linhaTempo';
import history from '~/servicos/history';
import notificacaoCategoria from '~/dtos/notificacaoCategoria';
import notificacaoStatus from '~/dtos/notificacaoStatus';
import servicoNotificacao from '~/servicos/Paginas/ServicoNotificacao';

const urlTelaNotificacoes = '/teste/notificacoes';

const DetalheNotificacao = ({ match }) => {
  const [idNotificacao, setIdNotificacao] = useState('');
  const [listaDeStatus, setListaDeStatus] = useState([]);
  const [aprovar, setAprovar] = useState(false);

  const [validacoes, setValidacoes] = useState(
    Yup.object({
      observacao: Yup.string().notRequired(),
    })
  );

  const [notificacao, setNotificacao] = useState({
    alteradoEm: '',
    alteradoPor: null,
    criadoEm: '',
    criadoPor: '',
    id: 0,
    mensagem: '',
    mostrarBotaoMarcarComoLido: false,
    mostrarBotoesDeAprovacao: false,
    situacao: '',
    tipo: '',
    titulo: '',
  });

  const buscaNotificacao = id => {
    api
      .get(`v1/notificacoes/${id}`)
      .then(resposta => setNotificacao(resposta.data))
      .catch(listaErros => erros(listaErros));
  };
  useEffect(() => {
    if (idNotificacao) {
      buscaNotificacao(idNotificacao);
    }
  }, [idNotificacao]);

  useEffect(() => {
    setIdNotificacao(match.params.id);
  }, [match.params.id]);

  useEffect(() => {
    const buscaLinhaTempo = () => {
      api
        .get(
          `v1/workflows/aprovacoes/notificacoes/${idNotificacao}/linha-tempo`
        )
        .then(resposta => {
          const status = resposta.data.map(item => {
            return {
              titulo: item.status,
              status: item.statusId,
              timestamp: item.alteracaoData,
              rf: item.alteracaoUsuarioRf,
              nome: item.alteracaoUsuario,
            };
          });
          setListaDeStatus(status);
        })
        .catch(listaErros => erros(listaErros));
    };
    if (
      notificacao &&
      notificacao.categoriaId === notificacaoCategoria.Workflow_Aprovacao
    ) {
      buscaLinhaTempo();
    }
  }, [notificacao]);

  const marcarComoLida = () => {
    const idsNotificacoes = [...idNotificacao];
    servicoNotificacao.marcarComoLida(idsNotificacoes, () =>
      history.push(urlTelaNotificacoes)
    );
  };

  const excluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir esta notificação?'
    );
    if (confirmado) {
      const idsNotificacoes = [...idNotificacao];
      servicoNotificacao.excluir(idsNotificacoes, () =>
        history.push(urlTelaNotificacoes)
      );
    }
  };

  const enviarAprovacao = async form => {
    const confirmado = await confirmar(
      'Atenção',
      `Você tem certeza que deseja "${
        aprovar ? 'Aceitar' : 'Recusar'
      }" esta notificação?`
    );
    if (confirmado) {
      const parametros = { ...form, aprova: aprovar };
      const url = `v1/workflows/aprovacoes/notificacoes/${idNotificacao}/aprova`;
      api
        .put(url, parametros)
        .then(() => {
          const mensagemSucesso = `Notificação "${
            aprovar ? 'Aceita' : 'Recusada'
          }" com sucesso.`;
          sucesso(mensagemSucesso);
          history.push(urlTelaNotificacoes);
        })
        .catch(listaErros => erros(listaErros));
    }
  };

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <Formik
        initialValues={{
          observacao: '',
        }}
        validationSchema={validacoes}
        onSubmit={values => enviarAprovacao(values)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <Card mtop="mt-2">
              <div className="col-md-2">
                <Button
                  label=""
                  color={cores.Colors.Azul}
                  className="mr-2 float-left"
                  icon="arrow-left"
                  border
                  type="button"
                  onClick={() => history.push(urlTelaNotificacoes)}
                />
              </div>
              <div className="col-md-10 d-flex justify-content-end pb-3">
                <>
                  <Button
                    label="Aceitar"
                    color={cores.Colors.Roxo}
                    disabled={!notificacao.mostrarBotoesDeAprovacao}
                    className="mr-2"
                    border={!notificacao.mostrarBotoesDeAprovacao}
                    type="button"
                    onClick={async e => {
                      setValidacoes(
                        Yup.object().shape({
                          observacao: Yup.string().notRequired(),
                        })
                      );
                      setAprovar(true);
                      form.validateForm().then(() => form.handleSubmit(e));
                    }}
                  />
                  <Button
                    label="Recusar"
                    color={cores.Colors.Roxo}
                    border
                    disabled={!notificacao.mostrarBotoesDeAprovacao}
                    className="mr-2"
                    type="button"
                    onClick={async e => {
                      setValidacoes(
                        Yup.object({
                          observacao: Yup.string()
                            .required('Observação obrigatória')
                            .max(
                              100,
                              'A observação deverá conter no máximo 100 caracteres'
                            ),
                        })
                      );
                      setAprovar(false);
                      form.validateForm().then(() => form.handleSubmit(e));
                    }}
                  />
                </>
                <Button
                  label="Marcar como lida"
                  color={cores.Colors.Azul}
                  border
                  className="mr-2"
                  disabled={!notificacao.mostrarBotaoMarcarComoLido}
                  onClick={marcarComoLida}
                />
                <Button
                  label="Excluir"
                  color={cores.Colors.Vermelho}
                  border
                  className="mr-2"
                  disabled={!notificacao.mostrarBotaoRemover}
                  onClick={excluir}
                />
              </div>
              <EstiloDetalhe>
                <div className="col-xs-12 col-md-12 col-lg-12">
                  <div className="row mg-bottom">
                    <div className="col-xs-12 col-md-12 col-lg-2 bg-id">
                      <div className="row">
                        <div className="col-xs-12 col-md-12 col-lg-12 text-center">
                          CÓDIGO
                        </div>
                        <div className="id-notificacao col-xs-12 col-md-12 col-lg-12 text-center">
                          {notificacao.codigo}
                        </div>
                      </div>
                    </div>
                    <div className="col-xs-12 col-md-12 col-lg-10">
                      <div className="row">
                        <div className="col-xs-12 col-md-12 col-lg-12">
                          <div className="notificacao-horario">
                            Notificação automática {notificacao.criadoEm}
                          </div>
                        </div>
                        <div className="col-xs-12 col-md-12 col-lg-12">
                          <div className="row">
                            <div className="col-xs-12 col-md-12 col-lg-4 titulo-coluna">
                              Tipo{' '}
                              <div className="conteudo-coluna">
                                {notificacao.tipo}
                              </div>
                            </div>
                            <div className="col-xs-12 col-md-12 col-lg-6 titulo-coluna">
                              Título
                              <div className="conteudo-coluna">
                                {notificacao.titulo}
                              </div>
                            </div>
                            <div className="col-xs-12 col-md-12 col-lg-2 titulo-coluna">
                              Situação
                              <div
                                className={`conteudo-coluna ${
                                  notificacao.statusId ===
                                  notificacaoStatus.Pendente
                                    ? 'texto-vermelho-negrito'
                                    : ''
                                }`}
                              >
                                {notificacao.situacao}
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <hr className="mt-hr" />
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12 mensagem">
                    MENSAGEM: {notificacao.mensagem}
                  </div>
                </div>
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12 obs">
                    <label>Observações</label>
                    <CampoTexto
                      name="observacao"
                      type="textarea"
                      form={form}
                      maxlength="100"
                      desabilitado={!notificacao.mostrarBotoesDeAprovacao}
                    />
                  </div>
                </div>
              </EstiloDetalhe>
              {notificacao.categoriaId ===
                notificacaoCategoria.Workflow_Aprovacao && (
                <EstiloLinhaTempo>
                  <div className="col-xs-12 col-md-12 col-lg-12">
                    <div className="row">
                      <div className="col-xs-12 col-md-12 col-lg-12">
                        <p>SITUAÇÃO DA NOTIFICAÇÃO</p>
                      </div>
                      <div className="col-xs-12 col-md-12 col-lg-12">
                        <LinhaTempo listaDeStatus={listaDeStatus} />
                      </div>
                    </div>
                  </div>
                </EstiloLinhaTempo>
              )}
            </Card>
          </Form>
        )}
      </Formik>
    </>
  );
};
export default DetalheNotificacao;
