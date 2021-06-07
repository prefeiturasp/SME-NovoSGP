import React, { useState, useRef, useEffect } from 'react';
import styled from 'styled-components';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import { Radio } from 'antd';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import { Colors, Base } from '~/componentes/colors';
import history from '~/servicos/history';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import CampoTexto from '~/componentes/campoTexto';
import { sucesso, erro, confirmar, erros } from '~/servicos/alertas';
import servicoEvento from '~/servicos/Paginas/Calendario/ServicoTipoEvento';

const TipoEventosForm = ({ match }) => {
  const botaoCadastrarRef = useRef();
  const campoDescricaoRef = useRef();

  const [idTipoEvento, setIdTipoEvento] = useState('');
  const [dadosTipoEvento, setDadosTipoEvento] = useState({
    descricao: '',
    letivo: undefined,
    localOcorrencia: undefined,
    concomitancia: true,
    tipoData: 1,
    dependencia: true,
    ativo: true,
  });
  const [desabilitarBotaoCadastrar, setDesabilitarBotaoCadastrar] = useState(
    true
  );
  const [modoEdicao, setModoEdicao] = useState(false);
  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '',
    alteradoPor: '',
    criadoEm: '',
    criadoPor: '',
  });

  const listaLetivo = [
    { valor: 1, descricao: 'Sim' },
    { valor: 2, descricao: 'Não' },
    { valor: 3, descricao: 'Opcional' },
  ];

  const listaLocalOcorrencia = [
    { valor: 1, descricao: 'UE' },
    { valor: 2, descricao: 'DRE' },
    { valor: 3, descricao: 'SME' },
    { valor: 4, descricao: 'SME/UE' },
    { valor: 5, descricao: 'Todos' },
  ];

  const Div = styled.div`
    .ant-radio-checked .ant-radio-inner {
      border-color: ${Base.Roxo};
    }
    .ant-radio-inner::after {
      background-color: ${Base.Roxo};
    }
  `;

  const Titulo = styled(Div)`
    color: ${Base.CinzaMako};
    font-size: 24px;
  `;

  const Rotulo = styled.label`
    color: ${Base.CinzaMako};
    font-size: 14px;
    font-weight: bold;
  `;

  const InseridoAlterado = styled(Div)`
    color: ${Base.CinzaMako};
    font-size: 10px;
    font-weight: bold;
    p {
      margin: 0;
    }
  `;

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setIdTipoEvento(match.params.id);
    }
  }, []);

  const [possuiEventos, setPossuiEventos] = useState(false);

  useEffect(() => {
    if (idTipoEvento) {
      api.get(`v1/calendarios/eventos/tipos/${idTipoEvento}`).then(resposta => {
        if (resposta && resposta.data) {
          setDadosTipoEvento({
            descricao: resposta.data.descricao,
            letivo: resposta.data.letivo.toString(),
            localOcorrencia: resposta.data.localOcorrencia.toString(),
            concomitancia: resposta.data.concomitancia,
            tipoData: resposta.data.tipoData,
            dependencia: resposta.data.dependencia,
            ativo: resposta.data.ativo,
          });
          setInseridoAlterado({
            alteradoEm: resposta.data.alteradoEm,
            alteradoPor: `${resposta.data.alteradoPor} (${resposta.data.alteradoRF})`,
            criadoEm: resposta.data.alteradoEm,
            criadoPor: `${resposta.data.criadoPor} (${resposta.data.criadoRF})`,
          });
          setModoEdicao(true);
          setPossuiEventos(resposta.data.possuiEventos);
        }
      });
    }
  }, [idTipoEvento]);

  const clicouBotaoVoltar = () => {
    history.push('/calendario-escolar/tipo-eventos');
  };

  const clicouBotaoCancelar = () => {
    setDadosTipoEvento({
      descricao: '',
      letivo: undefined,
      localOcorrencia: undefined,
      concomitancia: true,
      tipoData: 1,
      dependencia: true,
      ativo: true,
    });
    setDesabilitarBotaoCadastrar(true);
  };

  const clicouBotaoExcluir = async () => {
    if (idTipoEvento) {
      const confirmado = await confirmar(
        'Excluir tipo de calendário escolar',
        '',
        'Deseja realmente excluir este calendário?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const parametrosDelete = { data: [idTipoEvento] };
        const excluir = await api
          .delete('v1/calendarios/eventos/tipos', parametrosDelete)
          .catch(e => erros(e));
        if (excluir) {
          sucesso('Tipos de evento deletados com sucesso!');
          history.push('/calendario-escolar/tipo-eventos');
        }
      }
    }
  };

  const [validacoes] = useState(
    Yup.object({
      descricao: Yup.string().required('Digite o nome do tipo de evento'),
      localOcorrencia: Yup.string().required(
        'Selecione um local de ocorrência'
      ),
      letivo: Yup.string().required('Selecione um letivo'),
    })
  );

  const clicouBotaoCadastrar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
  };

  const cadastrarTipoEvento = async dados => {
    servicoEvento
      .salvar(idTipoEvento, dados)
      .then(() => {
        sucesso(
          `Tipo de evento ${
            modoEdicao ? 'atualizado' : 'cadastrado'
          } com sucesso!`
        );
        history.push('/calendario-escolar/tipo-eventos');
      })
      .catch(() => {
        erro(
          `Erro ao ${modoEdicao ? 'atualizar' : 'cadastrar'} o tipo de evento!`
        );
      });
  };

  const aoDigitarDescricao = e => {
    campoDescricaoRef.current.value = e.target.value;
  };

  useEffect(() => {
    if (dadosTipoEvento.descricao.length > 0)
      setDesabilitarBotaoCadastrar(false);
  }, [dadosTipoEvento.descricao]);

  const aoSelecionarLocalOcorrencia = local => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      localOcorrencia: local,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  const aoSelecionarLetivo = letivo => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      letivo,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  const aoSelecionarConcomitancia = concomitancia => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      concomitancia: concomitancia.target.value,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  const aoSelecionarTipoData = tipoData => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      tipoData: tipoData.target.value,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  const aoSelecionarDependencia = dependencia => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      dependencia: dependencia.target.value,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  const aoSelecionarSituacao = situacao => {
    setDadosTipoEvento({
      ...dadosTipoEvento,
      ativo: situacao.target.value,
      descricao: campoDescricaoRef.current.value,
    });
    setDesabilitarBotaoCadastrar(false);
  };

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">
          {idTipoEvento ? 'Alteração' : 'Cadastro'} de Tipo de Eventos
        </Titulo>
      </Grid>
      <Card className="rounded" mx="mx-auto">
        <Formik
          enableReinitialize
          initialValues={{
            descricao: dadosTipoEvento.descricao,
            letivo: dadosTipoEvento.letivo,
            localOcorrencia: dadosTipoEvento.localOcorrencia,
            concomitancia: dadosTipoEvento.concomitancia,
            tipoData: dadosTipoEvento.tipoData,
            dependencia: dadosTipoEvento.dependencia,
            ativo: dadosTipoEvento.ativo,
          }}
          onSubmit={dados => cadastrarTipoEvento(dados)}
          validationSchema={validacoes}
          validateOnBlur={false}
          validateOnChange={false}
        >
          {form => (
            <Div className="w-100">
              <Grid cols={12} className="d-flex justify-content-end mb-3">
                <Button
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  onClick={clicouBotaoVoltar}
                  border
                  className="mr-3"
                />
                <Button
                  label="Cancelar"
                  color={Colors.Roxo}
                  onClick={clicouBotaoCancelar}
                  border
                  bold
                  disabled={idTipoEvento}
                  className="mr-3"
                />
                <Button
                  label="Excluir"
                  color={Colors.Vermelho}
                  border
                  className="mr-3"
                  disabled={possuiEventos}
                  onClick={clicouBotaoExcluir}
                />
                <Button
                  label={idTipoEvento ? 'Alterar' : 'Cadastrar'}
                  color={Colors.Roxo}
                  onClick={e => clicouBotaoCadastrar(form, e)}
                  border
                  bold
                  disabled={desabilitarBotaoCadastrar}
                  ref={botaoCadastrarRef}
                />
              </Grid>
              <Grid cols={12}>
                <Form>
                  <Div className="row mb-4">
                    <Div className="col-6">
                      <Rotulo>Nome do tipo de evento</Rotulo>
                      <CampoTexto
                        form={form}
                        ref={campoDescricaoRef}
                        name="descricao"
                        id="descricao"
                        maxlength={100}
                        placeholder="Nome do evento"
                        type="input"
                        onChange={aoDigitarDescricao}
                        desabilitado={possuiEventos}
                        icon
                      />
                    </Div>
                    <Div className="col-4">
                      <Rotulo>Local de ocorrência</Rotulo>
                      <SelectComponent
                        form={form}
                        name="localOcorrencia"
                        id="localOcorrencia"
                        placeholder="Local de ocorrência"
                        valueOption="valor"
                        valueText="descricao"
                        lista={listaLocalOcorrencia}
                        onChange={aoSelecionarLocalOcorrencia}
                        disabled={possuiEventos}
                      />
                    </Div>
                    <Div className="col-2">
                      <Rotulo>Letivo</Rotulo>
                      <SelectComponent
                        form={form}
                        name="letivo"
                        id="letivo"
                        placeholder="Tipo"
                        valueOption="valor"
                        valueText="descricao"
                        lista={listaLetivo}
                        onChange={aoSelecionarLetivo}
                        disabled={possuiEventos}
                      />
                    </Div>
                  </Div>
                  <Div className="row">
                    <Div className="col-3">
                      <Rotulo>Permite concomitância</Rotulo>
                    </Div>
                    <Div className="col-3">
                      <Rotulo>Tipo de data</Rotulo>
                    </Div>
                    <Div className="col-3">
                      <Rotulo>Dependência</Rotulo>
                    </Div>
                    <Div className="col-3">
                      <Rotulo>Situação</Rotulo>
                    </Div>
                  </Div>
                  <Div className="row">
                    <Div className="col-3">
                      <Radio.Group
                        form={form}
                        value={dadosTipoEvento.concomitancia}
                        onChange={aoSelecionarConcomitancia}
                        disabled={possuiEventos}
                      >
                        <Div className="form-check form-check-inline">
                          <Radio value>Sim</Radio>
                        </Div>
                        <Div className="form-check form-check-inline">
                          <Radio value={false}>Não</Radio>
                        </Div>
                      </Radio.Group>
                    </Div>
                    <Div className="col-3">
                      <Radio.Group
                        form={form}
                        value={dadosTipoEvento.tipoData}
                        onChange={aoSelecionarTipoData}
                        disabled={possuiEventos}
                      >
                        <Div className="form-check form-check-inline">
                          <Radio value={1}>Única</Radio>
                        </Div>
                        <Div className="form-check form-check-inline">
                          <Radio value={2}>Início e fim</Radio>
                        </Div>
                      </Radio.Group>
                    </Div>
                    <Div className="col-3">
                      <Radio.Group
                        form={form}
                        value={dadosTipoEvento.dependencia}
                        onChange={aoSelecionarDependencia}
                        disabled={possuiEventos}
                      >
                        <Div className="form-check form-check-inline">
                          <Radio value>Sim</Radio>
                        </Div>
                        <Div className="form-check form-check-inline">
                          <Radio value={false}>Não</Radio>
                        </Div>
                      </Radio.Group>
                    </Div>
                    <Div className="col-3">
                      <Radio.Group
                        form={form}
                        value={dadosTipoEvento.ativo}
                        onChange={aoSelecionarSituacao}
                      >
                        <Div className="form-check form-check-inline">
                          <Radio value>Ativo</Radio>
                        </Div>
                        <Div className="form-check form-check-inline">
                          <Radio value={false}>Inativo</Radio>
                        </Div>
                      </Radio.Group>
                    </Div>
                  </Div>
                </Form>
              </Grid>
              <Grid cols={12}>
                <InseridoAlterado className="mt-4">
                  {inseridoAlterado.criadoPor && inseridoAlterado.criadoEm ? (
                    <p className="pt-2">
                      INSERIDO por {inseridoAlterado.criadoPor} em
                      {inseridoAlterado.criadoEm}
                    </p>
                  ) : (
                    ''
                  )}

                  {inseridoAlterado.alteradoPor &&
                  inseridoAlterado.alteradoEm ? (
                    <p>
                      ALTERADO por {inseridoAlterado.alteradoPor} em{' '}
                      {inseridoAlterado.alteradoEm}
                    </p>
                  ) : (
                    ''
                  )}
                </InseridoAlterado>
              </Grid>
            </Div>
          )}
        </Formik>
      </Card>
    </Div>
  );
};

export default TipoEventosForm;
