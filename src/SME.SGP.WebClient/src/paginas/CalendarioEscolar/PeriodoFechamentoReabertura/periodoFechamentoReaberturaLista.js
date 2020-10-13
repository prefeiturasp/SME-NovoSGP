import React, { useCallback, useEffect, useState } from 'react';
import { Form, Formik } from 'formik';
import * as moment from 'moment';
import { useSelector } from 'react-redux';

import {
  Button,
  Card,
  Colors,
  ListaPaginada,
  Loader,
  SelectAutocomplete,
} from '~/componentes';
import { Cabecalho, DreDropDown, UeDropDown } from '~/componentes-sgp';

import { URL_HOME } from '~/constantes';

import { modalidadeTipoCalendario, RotasDto } from '~/dtos';

import {
  confirmar,
  erros,
  history,
  ServicoCalendarios,
  ServicoFechamentoReabertura,
  sucesso,
  verificaSomenteConsulta,
} from '~/servicos';

import { CampoBimestre } from './periodoFechamentoReaberuraLista.css';

const PeriodoFechamentoReaberturaLista = () => {
  const usuario = useSelector(store => store.usuario);

  const permissoesTela =
    usuario.permissoes[RotasDto.PERIODO_FECHAMENTO_REABERTURA];
  let { anoLetivo } = usuario.turmaSelecionada;

  if (!anoLetivo) {
    anoLetivo = new Date().getFullYear();
  }

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    undefined
  );
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [idsReaberturasSelecionadas, setIdsReaberturasSelecionadas] = useState(
    []
  );

  const [ueSelecionada, setUeSelecionada] = useState(undefined);
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [filtroValido, setFiltroValido] = useState(false);
  const [filtro, setFiltro] = useState({});
  const [modalidadeTurma, setModalidadeTurma] = useState('');
  const [listaDres, setListaDres] = useState([]);
  const [listaTipoCalendario, setListaTipoCalendario] = useState([]);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');

  const criarCampoBimestre = (index, data) => {
    const bimestre = data[index];
    return bimestre ? (
      <CampoBimestre>
        <i className="fas fa-check" />
      </CampoBimestre>
    ) : (
        <></>
      );
  };

  const getColunasBimestreAnual = () => {
    return [
      {
        title: '1',
        dataIndex: 'bimestres',
        key: '1',
        render: data => criarCampoBimestre(0, data),
      },
      {
        title: '2',
        dataIndex: 'bimestres',
        key: '2',
        render: data => criarCampoBimestre(1, data),
      },
      {
        title: '3',
        dataIndex: 'bimestres',
        key: '3',
        render: data => criarCampoBimestre(2, data),
      },
      {
        title: '4',
        dataIndex: 'bimestres',
        key: '4',
        render: data => criarCampoBimestre(3, data),
      },
    ];
  };

  const getColunasBimestreSemestral = () => {
    return [
      {
        title: '1',
        dataIndex: 'bimestres',
        key: '1',
        render: data => criarCampoBimestre(0, data),
      },
      {
        title: '2',
        dataIndex: 'bimestres',
        key: '2',
        render: data => criarCampoBimestre(1, data),
      },
    ];
  };

  const [colunasBimestre, setColunasBimestre] = useState(
    getColunasBimestreAnual()
  );

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const onFiltrar = useCallback(() => {
    if (tipoCalendarioSelecionado) {
      const novoFiltro = {
        tipoCalendarioId: tipoCalendarioSelecionado,
      };

      if (dreSelecionada) {
        novoFiltro.dreCodigo = dreSelecionada;
        if (ueSelecionada) {
          novoFiltro.ueCodigo = ueSelecionada;
        }
      } else {
        novoFiltro.ueCodigo = '';
        setUeSelecionada('');
      }
      setFiltroValido(true);
      setFiltro({ ...novoFiltro });
    } else {
      setFiltroValido(false);
    }
  }, [dreSelecionada, tipoCalendarioSelecionado, ueSelecionada]);

  useEffect(() => {
    onFiltrar();
  }, [tipoCalendarioSelecionado, ueSelecionada, dreSelecionada, onFiltrar]);

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      setCarregandoTipos(true);

      const {
        data,
      } = await ServicoCalendarios.obterTiposCalendarioAutoComplete(
        pesquisaTipoCalendario
      );

      if (isSubscribed) {
        setListaTipoCalendario(data);
        setCarregandoTipos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [pesquisaTipoCalendario]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickExcluir = async () => {
    const confirmado = await confirmar(
      'Excluir Fechamento(s)',
      '',
      'Você tem certeza que deseja excluir este(s) registros(s)?',
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const idsDeletar = idsReaberturasSelecionadas.map(tipo => tipo.id);
      const excluir = await ServicoFechamentoReabertura.deletar(
        idsDeletar
      ).catch(e => erros(e));

      if (excluir && excluir.status == 200) {
        setIdsReaberturasSelecionadas([]);
        sucesso(excluir.data);
        onFiltrar();
      }
    }
  };

  const onClickNovo = () => {
    history.push(`/calendario-escolar/periodo-fechamento-reabertura/novo/${tipoCalendarioSelecionado ?? ' '}`);
  };

  const onClickEditar = item => {
    history.push(
      `/calendario-escolar/periodo-fechamento-reabertura/editar/${item.id}`
    );
  };

  const onSelecionarItems = ids => {
    setIdsReaberturasSelecionadas(ids);
  };

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('DD/MM/YYYY');
    }
    return <span> {dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Descrição',
      dataIndex: 'descricao',
      width: '65%',
    },
    {
      title: 'Início',
      dataIndex: 'dataInicio',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Fim',
      dataIndex: 'dataFim',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Bimestres',
      children: colunasBimestre,
    },
  ];

  const selecionaTipoCalendario = (descricao, form) => {
    const tipo = listaTipoCalendario?.find(t => t.descricao === descricao);
    if (Number(tipo?.id) || !tipo?.id) {
      const value =
        tipo?.modalidade === modalidadeTipoCalendario.FUNDAMENTAL_MEDIO
          ? getColunasBimestreAnual
          : getColunasBimestreSemestral;
      setColunasBimestre(value);
      setValorTipoCalendario(descricao);
      form.setFieldValue('ueId', '');
      setUeSelecionada('');
      if (listaDres && listaDres.length > 1) {
        form.setFieldValue('dreId', '');
        setDreSelecionada('');
      }
    }
    setTipoCalendarioSelecionado(tipo?.id);
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      setPesquisaTipoCalendario(descricao);
    }
  };

  return (
    <>
      <Cabecalho pagina="Período de Fechamento (Reabertura)" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={{
            ueId: undefined,
            dreId: undefined,
            tipoCalendarioId: undefined,
          }}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12">
              <div className="row mb-4">
                <div className="col-md-12 d-flex justify-content-end pb-4">
                  <Button
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={onClickVoltar}
                  />
                  <Button
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    onClick={onClickExcluir}
                    disabled={
                      !permissoesTela.podeExcluir ||
                      (idsReaberturasSelecionadas &&
                        idsReaberturasSelecionadas.length < 1)
                    }
                  />
                  <Button
                    label="Novo"
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    onClick={onClickNovo}
                    disabled={somenteConsulta || !permissoesTela.podeIncluir}
                  />
                </div>
                <div className="col-md-12 mb-2">
                  <Loader loading={carregandoTipos} tip="">
                    <div style={{ maxWidth: '300px' }}>
                      <SelectAutocomplete
                        hideLabel
                        showList
                        isHandleSearch
                        placeholder="Selecione um tipo de calendário"
                        className="col-md-12"
                        name="tipoCalendarioId"
                        id="tipoCalendarioId"
                        lista={listaTipoCalendario}
                        valueField="id"
                        textField="descricao"
                        onSelect={valor => selecionaTipoCalendario(valor, form)}
                        onChange={valor => selecionaTipoCalendario(valor, form)}
                        handleSearch={handleSearch}
                        value={valorTipoCalendario}
                      />
                    </div>
                  </Loader>
                </div>
                <div className="col-md-6 mb-2">
                  {tipoCalendarioSelecionado && (
                    <DreDropDown
                      label="Diretoria Regional de Educação (DRE)"
                      form={form}
                      onChange={(dreId, dres) => {
                        setDreSelecionada(dreId);
                        setListaDres(dres);
                        const tipoSelecionado = listaTipoCalendario.find(
                          item => item.id === tipoCalendarioSelecionado
                        );
                        if (tipoSelecionado && tipoSelecionado.modalidade) {
                          const modalidadeT = ServicoCalendarios.converterModalidade(
                            tipoSelecionado.modalidade
                          );
                          setModalidadeTurma(modalidadeT);
                        } else {
                          setModalidadeTurma('');
                        }
                      }}
                      desabilitado={false}
                    />
                  )}
                </div>
                <div className="col-md-6 pb-2">
                  {tipoCalendarioSelecionado && (
                    <UeDropDown
                      dreId={form.values.dreId}
                      label="Unidade Escolar (UE)"
                      form={form}
                      url=""
                      onChange={ueId => setUeSelecionada(ueId)}
                      desabilitado={false}
                      modalidade={modalidadeTurma}
                    />
                  )}
                </div>
                <div className="col-md-12 pt-2">
                  {tipoCalendarioSelecionado ? (
                    <ListaPaginada
                      url="v1/fechamentos/reaberturas"
                      id="lista-fechamento-reaberturas"
                      colunaChave="id"
                      colunas={colunas}
                      filtro={filtro}
                      onClick={onClickEditar}
                      multiSelecao
                      selecionarItems={onSelecionarItems}
                      filtroEhValido={filtroValido}
                    />
                  ) : (
                      ''
                    )}
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default PeriodoFechamentoReaberturaLista;
