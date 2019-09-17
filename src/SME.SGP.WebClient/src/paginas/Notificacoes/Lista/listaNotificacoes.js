import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import Cabecalho from '~/componentes-sgp/cabecalho';
import api from '~/servicos/api';

import { EstiloLista } from './estiloLista';
import CampoTexto from '~/componentes/campoTexto';
import { erros, erro, sucesso, confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';
import servicoNotificacao from '~/servicos/Paginas/ServicoNotificacao';

export default function NotificacoesLista() {
  const [idNotificacoesSelecionadas, setIdNotificacoesSelecionadas] = useState(
    []
  );
  const [listaNotificacoes, setListaNotificacoes] = useState([]);
  const [listaCategorias, setListaCategorias] = useState([]);
  const [listaStatus, setListaStatus] = useState([]);
  const [listaTipos, setTipos] = useState([]);

  const [turmaSelecionada, setTurmaSelecionada] = useState();
  const [statusSelecionado, setStatusSelecionado] = useState();
  const [categoriaSelecionada, setCategoriaSelecionada] = useState();
  const [tipoSelecionado, setTipoSelecionado] = useState();
  const [tituloSelecionado, setTituloSelecionado] = useState();
  const [codigoSelecionado, setCodigoSelecionado] = useState();

  const columns = [
    {
      title: 'Código',
      dataIndex: 'codigo',
      render: (text, row) => (
        <a onClick={() => editarNotificacao(row)}>{text}</a>
      ),
    },
    {
      title: 'Tipo',
      dataIndex: 'tipo',
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
    },
    {
      title: 'Situação',
      dataIndex: 'descricaoStatus',
    },
    {
      title: 'Data/Hora',
      dataIndex: 'data',
    },
  ];

  const usuario = useSelector(store => store.usuario);

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

  const listaSelectTurma = [
    { id: 1, descricao: 'Todas as turmas' },
    { id: 2, descricao: 'Turma selecionada' },
  ];

  function onSelectRow(row) {
    setIdNotificacoesSelecionadas(row);
  }

  function onChangeTurma(turma) {
    console.log(`turma = ${turma}`);
    setTurmaSelecionada(turma);
  }

  function onChangeStatus(status) {
    console.log(`status = ${status}`);
    setStatusSelecionado(status);
  }

  function onChangeCategoria(categoria) {
    console.log(`categoria = ${categoria}`);
    setCategoriaSelecionada(categoria);
  }

  function onChangeTitulo(titulo) {
    setTituloSelecionado(titulo.target.value);
  }

  function onChangeCodigo(codigo) {
    setCodigoSelecionado(codigo.target.value);
  }

  function editarNotificacao(row) {
    history.push(`/notificacoes/${row.id}`);
  }

  async function onClickFiltrar() {
    const paramsQuery = {
      ano: usuario.turmaSelecionada.ano,
      anoLetivo: usuario.turmaSelecionada.anoLetivo,
      categoria: categoriaSelecionada,
      codigo: codigoSelecionado || null,
      dreId: usuario.turmaSelecionada.codDre,
      status: statusSelecionado,
      tipo: tipoSelecionado,
      titulo: tituloSelecionado || null,
      turmaId: usuario.turmaSelecionada.codTurma,
      ueId: usuario.turmaSelecionada.codEscola,
      usuarioId: '7208626', // TODO Mock
    };
    const listaNotifi = await api.get('v1/notificacoes', {
      params: paramsQuery,
    });
    console.log(listaNotifi);
    setListaNotificacoes(listaNotifi.data);
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
      servicoNotificacao.excluir(idNotificacoesSelecionadas, () =>
        onClickFiltrar()
      );
    }
  }

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <EstiloLista>
        <div className="col-md-2 pb-2">
          <SelectComponent
            name="turma-noti"
            id="turma-noti"
            lista={listaSelectTurma}
            valueOption="id"
            valueText="descricao"
            onChange={onChangeTurma}
            valueSelect={turmaSelecionada || []}
            placeholder="Turma"
          />
        </div>
        <div className="col-md-2 pb-2">
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
        <div className="col-md-2 pb-2">
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
        <div className="col-md-3 pb-2">
          <CampoTexto
            placeholder="Título"
            onChange={onChangeTitulo}
            value={tituloSelecionado}
          />
        </div>
        <div className="col-md-2 pb-2">
          <CampoTexto
            placeholder="Código"
            onChange={onChangeCodigo}
            value={codigoSelecionado}
          />
        </div>

        <div className="col-md-1 pb-2">
          <Button
            label="Filtrar"
            color={Colors.Azul}
            border
            className="mb-2 "
            onClick={onClickFiltrar}
          />
        </div>

        <div className="col-md-12 ">
          <Button
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mb-2 ml-2 float-right"
            onClick={excluir}
          />
          <Button
            label="Marcar como lida"
            color={Colors.Azul}
            border
            className="mb-2 ml-2 float-right"
            onClick={marcarComoLida}
          />
          {/* <Button
            label="Recusar"
            color={Colors.Roxo}
            border
            className="mb-2 ml-2 float-right"
            onClick={() => {}}
          />
          <Button
            label="Aceitar"
            color={Colors.Roxo}
            border
            className="mb-2 ml-2 float-right"
            onClick={() => {}}
          /> */}
          {/* <Button
            label="Filtrar"
            color={Colors.Azul}
            border
            className="mb-2 float-right"
            onClick={onClickFiltrar}
          /> */}
        </div>
        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-notificacoes"
            selectedRowKeys={idNotificacoesSelecionadas}
            onSelectRow={onSelectRow}
            columns={columns}
            dataSource={listaNotificacoes}
            selectMultipleRows
          />
        </div>
      </EstiloLista>
    </>
  );
}
