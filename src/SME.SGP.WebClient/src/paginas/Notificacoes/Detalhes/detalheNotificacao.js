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
              rf: '7972324',
              nome: item.alteracaoUsuario,
            };
          });
          setListaDeStatus(status);
        })
        .catch(listaErros => erros(listaErros));
    };
    if (notificacao && notificacao.mostrarBotoesDeAprovacao) {
      buscaLinhaTempo();
    }
  }, [notificacao]);

  const salvar = form => {
    debugger;
  };

  const marcarComoLida = () => {
    const idsNotificacoes = [...idNotificacao];
    api
      .put('v1/notificacoes/status/lida', idsNotificacoes)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(resultado => {
            if (resultado.sucesso) {
              sucesso(resultado.mensagem);
            } else {
              erro(resultado.mensagem);
            }
          });
        }
        history.push('/notificacoes');
      })
      .catch(listaErros => erros(listaErros));
  };

  const excluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir estas notificações?'
    );
    if (confirmado) {
      const idsNotificacoes = [...idNotificacao];
      api
        .delete('v1/notificacoes/', idsNotificacoes)
        .then(resposta => {
          if (resposta.data) {
            resposta.data.forEach(resultado => {
              if (resultado.sucesso) {
                sucesso(resultado.mensagem);
              } else {
                erro(resultado.mensagem);
              }
            });
          }
          history.push('/notificacoes');
        })
        .catch(listaErros => erros(listaErros));
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
      form.aprova = aprovar;
      api
        .put(
          `v1/workflows/aprovacoes/notificacoes/${idNotificacao}/aprova`,
          form
        )
        .then(resposta => {
          sucesso(
            `Notificação "${aprovar ? 'Aceita' : 'Recusada'}" com sucesso.`
          );
          history.push('/notificacoes');
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
              <div className="col-md-12 d-flex justify-content-end pb-3">
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
                      form.validateForm().then(r => form.handleSubmit(e));
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
                          observacao: Yup.string().required(
                            'Observação obrigatória'
                          ),
                        })
                      );
                      setAprovar(false);
                      form.validateForm().then(r => form.handleSubmit(e));
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
                  border
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
                          ID
                        </div>
                        <div className="id-notificacao col-xs-12 col-md-12 col-lg-12 text-center">
                          {notificacao.id}
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
                                  notificacao.statusId == 1
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
                      desabilitado={!notificacao.mostrarBotoesDeAprovacao}
                    />
                  </div>
                </div>
              </EstiloDetalhe>
              {notificacao.mostrarBotoesDeAprovacao && (
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
