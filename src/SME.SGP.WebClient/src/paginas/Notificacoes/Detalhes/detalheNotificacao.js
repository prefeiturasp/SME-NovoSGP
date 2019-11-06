import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
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

const urlTelaNotificacoes = '/notificacoes';

const DetalheNotificacao = ({ match }) => {
  const [idNotificacao, setIdNotificacao] = useState('');
  const [listaDeStatus, setListaDeStatus] = useState([]);
  const [aprovar, setAprovar] = useState(false);

  const titulosNiveis = ['', 'Aguardando aceite', 'Aceita', 'Recusada', 'Sem status'];

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

  const buscaNotificacao = id => {
    api
      .get(`v1/notificacoes/${id}`)
      .then(resposta => setNotificacao(resposta.data))
      .catch(listaErros => erros(listaErros));
  };

  useEffect(() => {
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

  useEffect(() => {
    const buscaLinhaTempo = () => {
      api
        .get(
          `v1/workflows/aprovacoes/notificacoes/${idNotificacao}/linha-tempo`
        )
        .then(resposta => {
          const status = resposta.data.map(item => {
            return {
              titulo: titulosNiveis[item.statusId],
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
    if (notificacao.categoriaId === notificacaoCategoria.Aviso) {
      if (usuario.rf.length > 0)
        servicoNotificacao.buscaNotificacoesPorAnoRf(2019, usuario.rf);
    }
  }, [notificacao]);

  const marcarComoLida = () => {
    const idsNotificacoes = [idNotificacao];
    servicoNotificacao.marcarComoLida(idsNotificacoes, () => {
      history.push(urlTelaNotificacoes);
      if (usuario.rf.length > 0)
        servicoNotificacao.buscaNotificacoesPorAnoRf(2019, usuario.rf);
    });
  };

  const excluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir esta notificação?'
    );
    if (confirmado) {
      const idsNotificacoes = [idNotificacao];
      servicoNotificacao.excluir(idsNotificacoes, () => {
        history.push(urlTelaNotificacoes);
        if (usuario.rf.length > 0)
          servicoNotificacao.buscaNotificacoesPorAnoRf(2019, usuario.rf);
      });
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

  const formatarData = dataString => {
    const data = new Date(dataString);
    if (data && data.getDate()) {
      const mes = transformaUnidadeData(data.getMonth().toString());
      const dia = transformaUnidadeData(data.getDate().toString());
      const hora = transformaUnidadeData(data.getHours().toString());
      const minutos = transformaUnidadeData(data.getMinutes().toString());
      return `${dia}/${mes}/${data.getFullYear()}, ${hora}:${minutos}`;
    }
    return dataString;
  };

  const transformaUnidadeData = unidade => {
    if (unidade.length === 1) {
      unidade = '0' + unidade;
    }
    return unidade;
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
            <Card mtop="mt-2">
              <div className="col-md-2">
                <Button
                  label=""
                  color={cores.Colors.Azul}
                  className="mr-2 float-left"
                  icon="arrow-left"
                  border
                  type="button"
                  width="44px"
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
                          2019,
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
                            Notificação automática{' '}
                            {formatarData(notificacao.criadoEm)}
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
                                {
                                  notificacao.statusId ===
                                  notificacaoStatus.Pendente
                                  ? 'Não Lida' : notificacao.situacao}
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
          </Form>
        )}
      </Formik>
    </>
  );
};
export default DetalheNotificacao;
