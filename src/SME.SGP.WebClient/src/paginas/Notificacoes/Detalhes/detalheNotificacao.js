import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
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
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import { Loader } from '~/componentes';

const urlTelaNotificacoes = '/notificacoes';

const DetalheNotificacao = ({ match }) => {
  const [idNotificacao, setIdNotificacao] = useState('');
  const [listaDeStatus, setListaDeStatus] = useState([]);
  const [carregandoTela, setCarregandoTela] = useState(false);
  const [aprovar, setAprovar] = useState(false);

  const titulosNiveis = [
    '',
    'Aguardando aceite',
    'Aceita',
    'Recusada',
    'Sem status',
  ];

  const usuario = useSelector(state => state.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.NOTIFICACOES];

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

  const buscaNotificacao = async id => {
    try {
      setCarregandoTela(true);
      const { data, status } = await api.get(`v1/notificacoes/${id}`);
      if (data && status === 200) {
        setNotificacao(data);
        setCarregandoTela(false);
      }
    } catch (listaErros) {
      setCarregandoTela(false);
      erros(listaErros);
    }
  };

  useEffect(() => {
    setBreadcrumbManual(match.url, 'Detalhes', '/notificacoes');
    verificaSomenteConsulta(permissoesTela);
  }, []);

  useEffect(() => {
    if (idNotificacao) {
      buscaNotificacao(idNotificacao);
    }
  }, [idNotificacao]);

  useEffect(() => {
    setIdNotificacao(match.params.id);
  }, [match.params.id]);

  const anoAtual = window.moment().format('YYYY');

  useEffect(() => {
    const buscaLinhaTempo = async () => {
      try {
        setCarregandoTela(true);
        const { data, status } = await api.get(
          `v1/workflows/aprovacoes/notificacoes/${idNotificacao}/linha-tempo`
        );
        if (data && status === 200) {
          setListaDeStatus(
            data.map(item => ({
              titulo: titulosNiveis[item.statusId],
              status: item.statusId,
              timestamp: item.alteracaoData,
              rf: item.alteracaoUsuarioRf,
              nome: item.alteracaoUsuario,
            }))
          );
          setCarregandoTela(false);
        }
      } catch (listaErros) {
        setCarregandoTela(false);
        erros(listaErros);
      }
    };

    if (
      notificacao &&
      notificacao.categoriaId === notificacaoCategoria.Workflow_Aprovacao
    ) {
      buscaLinhaTempo();
    }
    if (notificacao.categoriaId === notificacaoCategoria.Aviso) {
      if (usuario.rf.length > 0)
        servicoNotificacao.buscaNotificacoesPorAnoRf(anoAtual, usuario.rf);
    }
  }, [notificacao]);

  const marcarComoLida = async () => {
    try {
      setCarregandoTela(true);
      const idsNotificacoes = [idNotificacao];
      const { data, status } = await servicoNotificacao.marcarComoLidaNot(
        idsNotificacoes
      );
      if (data && status === 200) {
        data.forEach(resultado => {
          if (resultado.sucesso) {
            sucesso(resultado.mensagem);
          } else {
            erro(resultado.mensagem);
          }
        });

        history.push(urlTelaNotificacoes);
        if (usuario.rf.length > 0)
          servicoNotificacao.buscaNotificacoesPorAnoRf(anoAtual, usuario.rf);

        setCarregandoTela(false);
      }
    } catch (error) {
      setCarregandoTela(false);
      erro('Não foi possível marcar notificação como lida!');
    }
  };

  const excluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir esta notificação?'
    );
    if (confirmado) {
      try {
        setCarregandoTela(true);
        const idsNotificacoes = [idNotificacao];
        const { data, status } = await servicoNotificacao.excluirNot(
          idsNotificacoes
        );
        if (data && status === 200) {
          data.forEach(resultado => {
            if (resultado.sucesso) {
              sucesso(resultado.mensagem);
            } else {
              erro(resultado.mensagem);
            }
          });

          history.push(urlTelaNotificacoes);
          if (usuario.rf.length > 0) {
            servicoNotificacao.buscaNotificacoesPorAnoRf(anoAtual, usuario.rf);
          }

          setCarregandoTela(false);
        }
      } catch (error) {
        setCarregandoTela(false);
        erro('Não foi possível excluir a notificação!');
      }
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
      try {
        setCarregandoTela(true);
        const parametros = { ...form, aprova: aprovar };
        const { data, status } = await servicoNotificacao.enviarAprovacaoNot(
          idNotificacao,
          parametros
        );
        if ((data !== null || data !== undefined) && status === 200) {
          sucesso(
            `Notificação ${aprovar ? `Aceita` : `Recusada`} com sucesso.`
          );
          setCarregandoTela(false);
          history.push(urlTelaNotificacoes);
        }
      } catch (listaErros) {
        setCarregandoTela(false);
        erros(listaErros);
      }
    }
  };

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <Formik
        enableReinitialize
        initialValues={{
          observacao: notificacao.observacao || '',
        }}
        validationSchema={validacoes}
        onSubmit={values => enviarAprovacao(values)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <Loader loading={carregandoTela} tip="Carregando...">
              <Card mtop="mt-2">
                <div className="col-md-2">
                  <Button
                    label="Voltar"
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
                      disabled={
                        !notificacao.mostrarBotoesDeAprovacao ||
                        !permissoesTela.podeAlterar
                      }
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
                        if (usuario.rf.length > 0)
                          servicoNotificacao.buscaNotificacoesPorAnoRf(
                            anoAtual,
                            usuario.rf
                          );
                        form.validateForm().then(() => form.handleSubmit(e));
                      }}
                    />
                    <Button
                      label="Recusar"
                      color={cores.Colors.Roxo}
                      border
                      disabled={
                        !notificacao.mostrarBotoesDeAprovacao ||
                        !permissoesTela.podeAlterar
                      }
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
                    disabled={
                      !notificacao.mostrarBotaoMarcarComoLido ||
                      !permissoesTela.podeAlterar
                    }
                    onClick={marcarComoLida}
                  />
                  <Button
                    label="Excluir"
                    color={cores.Colors.Vermelho}
                    border
                    className="mr-2"
                    disabled={
                      !notificacao.mostrarBotaoRemover ||
                      !permissoesTela.podeExcluir
                    }
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
                              {`Notificação automática ${notificacao.criadoEm.substr(
                                0,
                                9
                              )}`}
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
                                  {notificacao.statusId ===
                                  notificacaoStatus.Pendente
                                    ? 'Não Lida'
                                    : notificacao.situacao}
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
                      MENSAGEM:{' '}
                      <span
                        dangerouslySetInnerHTML={{
                          __html: notificacao.mensagem,
                        }}
                      />
                    </div>
                  </div>
                  {notificacao.categoriaId ===
                    notificacaoCategoria.Workflow_Aprovacao && (
                    <div className="row">
                      <div className="col-xs-12 col-md-12 col-lg-12 obs">
                        <label>Observações</label>
                        <CampoTexto
                          name="observacao"
                          type="textarea"
                          form={form}
                          maxlength="100"
                          desabilitado={
                            !notificacao.mostrarBotoesDeAprovacao ||
                            !permissoesTela.podeAlterar
                          }
                        />
                      </div>
                    </div>
                  )}
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
            </Loader>
          </Form>
        )}
      </Formik>
    </>
  );
};
export default DetalheNotificacao;
