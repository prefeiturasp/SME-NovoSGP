import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as moment from 'moment';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import { confirmar } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import servicoNotificacao from '~/servicos/Paginas/ServicoNotificacao';

import { EstiloLista } from './estiloLista';
import notificacaoStatus from '~/dtos/notificacaoStatus';
import CampoTextoBusca from '~/componentes/campoTextoBusca';
import { URL_HOME } from '~/constantes/url';

export default function NotificacoesLista() {
  const [idNotificacoesSelecionadas, setIdNotificacoesSelecionadas] = useState(
    []
  );
  const [listaNotificacoes, setListaNotificacoes] = useState([]);
  const [listaCategorias, setListaCategorias] = useState([]);
  const [listaStatus, setListaStatus] = useState([]);
  const [listaTipos, setTipos] = useState([]);

  const [dropdownTurmaSelecionada, setTurmaSelecionada] = useState();
  const [statusSelecionado, setStatusSelecionado] = useState();
  const [categoriaSelecionada, setCategoriaSelecionada] = useState();
  const [tipoSelecionado, setTipoSelecionado] = useState();
  const [tituloSelecionado, setTituloSelecionado] = useState();
  const [codigoSelecionado, setCodigoSelecionado] = useState();
  const [desabilitarBotaoExcluir, setDesabilitarBotaoExcluir] = useState(true);
  const [desabilitarBotaoMarcarLido, setDesabilitarBotaoMarcarLido] = useState(
    true
  );
  const [desabilitarTurma, setDesabilitarTurma] = useState(true);
  const [colunasTabela, setColunasTabela] = useState([]);

  const usuario = useSelector(store => store.usuario);

  useEffect(() => {
    const colunas = [
      {
        title: 'Código',
        dataIndex: 'codigo',
        render: (text, row) => montarLinhasTabela(text, row),
      },
      {
        title: 'Tipo',
        dataIndex: 'tipo',
        render: (text, row) => montarLinhasTabela(text, row),
      },
      {
        title: 'Categoria',
        dataIndex: 'descricaoCategoria',
        render: (text, row) => montarLinhasTabela(text, row),
      },
      {
        title: 'Título',
        dataIndex: 'titulo',
        render: (text, row) => montarLinhasTabela(text, row),
      },
      {
        title: 'Situação',
        dataIndex: 'descricaoStatus',
        render: (text, row) => montarLinhasTabela(text, row, true),
      },
      {
        title: 'Data/Hora',
        dataIndex: 'data',
        width: 200,
        render: (text, row) => {
          const dataFormatada = moment(text).format('DD/MM/YYYY HH:mm:ss');
          return montarLinhasTabela(dataFormatada, row);
        },
      },
    ];

    setColunasTabela(colunas);
  }, []);

  useEffect(() => {
    async function carregarListas() {
      const status = await api.get('v1/notificacoes/status');
      setListaStatus(status.data);

      const categorias = await api.get('v1/notificacoes/categorias');
      setListaCategorias(categorias.data);

      const tipos = await api.get('v1/notificacoes/tipos');
      setTipos(tipos.data);
    }

    carregarListas();
  }, []);

  useEffect(() => {
    if (
      usuario &&
      usuario.turmaSelecionada &&
      usuario.turmaSelecionada.length
    ) {
      setDesabilitarTurma(false);
    } else {
      setDesabilitarTurma(true);
      setTurmaSelecionada('');
    }
    onClickFiltrar();
  }, [usuario.turmaSelecionada]);

  useEffect(() => {
    onClickFiltrar();
  }, [
    statusSelecionado,
    dropdownTurmaSelecionada,
    categoriaSelecionada,
    tipoSelecionado,
    tituloSelecionado,
  ]);

  const listaSelectTurma = [
    { id: 1, descricao: 'Todas as turmas' },
    { id: 2, descricao: 'Turma selecionada' },
  ];

  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  function montarLinhasTabela(text, row, colunaSituacao) {
    return row.status === notificacaoStatus.Pendente ? (
      colunaSituacao ? (
        <span className="cor-vermelho font-weight-bold text-uppercase">
          {statusLista[row.status]}
        </span>
      ) : (
          <span className="cor-novo-registro-lista">{text}</span>
        )
    ) : (
        text
      );
  }

  function onSelectRow(ids) {

    if (ids && ids.length > 0) {
      const notifSelecionadas = listaNotificacoes.filter(noti => {
        return ids.includes(noti.id);
      });

      const naoPodeRemover = notifSelecionadas.find(item => !item.podeRemover);
      if (naoPodeRemover) {
        setDesabilitarBotaoExcluir(true);
      } else {
        setDesabilitarBotaoExcluir(false);
      }

      const naoPodeMarcarLido = notifSelecionadas.find(
        item => !item.podeMarcarComoLida
      );
      if (naoPodeMarcarLido) {
        setDesabilitarBotaoMarcarLido(true);
      } else {
        setDesabilitarBotaoMarcarLido(false);
      }
    } else {
      setDesabilitarBotaoExcluir(true);
      setDesabilitarBotaoMarcarLido(true);
    }

    setIdNotificacoesSelecionadas(ids);
  }

  function onClickRow(row) {
    onClickEditar(row.id);
  }

  function onChangeTurma(turma) {
    setTurmaSelecionada(turma);
  }

  function onChangeStatus(status) {
    setStatusSelecionado(status);
  }

  function onChangeCategoria(categoria) {
    setCategoriaSelecionada(categoria);
  }

  function onChangeTitulo(titulo) {
    setTituloSelecionado(titulo.target.value);
  }

  function onSearchCodigo() {
    onClickFiltrar();
  }

  function onChangeCodigo(codigo) {
    setCodigoSelecionado(codigo.target.value);
  }

  function onChangeTipo(tipo) {
    setTipoSelecionado(tipo);
  }

  function onClickEditar(id) {
    history.push(`/notificacoes/${id}`);
  }

  async function onClickFiltrar() {
    const paramsQuery = {
      categoria: categoriaSelecionada,
      codigo: codigoSelecionado || null,
      status: statusSelecionado,
      tipo: tipoSelecionado,
      titulo: tituloSelecionado || null,
      usuarioRf: usuario.rf,
      anoLetivo: usuario.turmaSelecionada[0].anoLetivo
    };
    if (dropdownTurmaSelecionada && dropdownTurmaSelecionada == '2') {
      if (usuario.turmaSelecionada && usuario.turmaSelecionada.length) {
        paramsQuery.ano = usuario.turmaSelecionada[0].ano;
        paramsQuery.dreId = usuario.turmaSelecionada[0].codDre;
        paramsQuery.ueId = usuario.turmaSelecionada[0].codEscola;
      }
      if (
        usuario.turmaSelecionada &&
        usuario.turmaSelecionada.length &&
        !desabilitarTurma
      ) {
        paramsQuery.turmaId = usuario.turmaSelecionada[0].codEscola;
      }
    }
    const listaNotifi = await api.get('v1/notificacoes', {
      params: paramsQuery,
    });
    setListaNotificacoes(listaNotifi.data);
    setIdNotificacoesSelecionadas([]);
    onSelectRow([]);
  }

  function marcarComoLida() {
    servicoNotificacao.marcarComoLida(idNotificacoesSelecionadas, () =>
      onClickFiltrar()
    );
  }

  async function excluir() {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir estas notificações?'
    );
    if (confirmado) {
      servicoNotificacao.excluir(idNotificacoesSelecionadas, () => {
        onClickFiltrar();
        setIdNotificacoesSelecionadas([]);
      });
    }
  }

  function quandoTeclaParaBaixoPesquisaCodigo(e) {
    if (e.key === 'e') e.preventDefault();
  }

  function quandoClicarVoltar() {
    history.push(URL_HOME);
  }

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <EstiloLista>
        <div className="col-md-6 pb-3">
          <CampoTexto
            placeholder="Título"
            onChange={onChangeTitulo}
            value={tituloSelecionado}
          />
        </div>
        <div className="col-md-6 pb-3">
          <CampoTextoBusca
            placeholder="Código"
            onSearch={onSearchCodigo}
            onChange={onChangeCodigo}
            value={codigoSelecionado}
            onKeyDown={quandoTeclaParaBaixoPesquisaCodigo}
            type="number"
          />
        </div>
        <div className="col-md-3 pb-3">
          <SelectComponent
            name="turma-noti"
            id="turma-noti"
            lista={listaSelectTurma}
            valueOption="id"
            valueText="descricao"
            onChange={onChangeTurma}
            valueSelect={dropdownTurmaSelecionada || []}
            placeholder="Turma"
            disabled={desabilitarTurma}
          />
        </div>
        <div className="col-md-3 pb-3">
          <SelectComponent
            name="status-noti"
            id="status-noti"
            lista={listaStatus}
            valueOption="id"
            valueText="descricao"
            onChange={onChangeStatus}
            valueSelect={statusSelecionado || []}
            placeholder="Filtrar por"
          />
        </div>
        <div className="col-md-3 pb-3">
          <SelectComponent
            name="categoria-noti"
            id="categoria-noti"
            lista={listaCategorias}
            valueOption="id"
            valueText="descricao"
            onChange={onChangeCategoria}
            valueSelect={categoriaSelecionada || []}
            placeholder="Categoria"
          />
        </div>
        <div className="col-md-3 pb-3">
          <SelectComponent
            name="tipo-noti"
            id="tipo-noti"
            lista={listaTipos}
            valueOption="id"
            valueText="descricao"
            onChange={onChangeTipo}
            valueSelect={tipoSelecionado || []}
            placeholder="Tipo"
          />
        </div>
        <div className="col-md-12 ">
          <Button
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mb-2 ml-2 float-right"
            onClick={excluir}
            disabled={desabilitarBotaoExcluir}
          />
          <Button
            label="Marcar como lida"
            color={Colors.Azul}
            border
            className="mb-2 ml-2 float-right"
            onClick={marcarComoLida}
            disabled={desabilitarBotaoMarcarLido}
          />
          <Button
            label="Voltar"
            color={Colors.Azul}
            border
            className="mb-2 float-right"
            onClick={quandoClicarVoltar}
          />
        </div>
        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-notificacoes"
            selectedRowKeys={idNotificacoesSelecionadas}
            onSelectRow={onSelectRow}
            onClickRow={onClickRow}
            columns={colunasTabela}
            dataSource={listaNotificacoes}
            selectMultipleRows
          />
        </div>
      </EstiloLista>
    </>
  );
}
