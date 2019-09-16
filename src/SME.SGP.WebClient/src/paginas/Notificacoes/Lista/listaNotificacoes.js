import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import InputComponent from '~/componentes/input';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import { NovoSGP, Titulo } from '~/helpers/styledComponentsGeneric';
import api from '~/servicos/api';

import { EstiloLista } from './estiloLista';

export default function NotificacoesLista() {
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [listaNotificacoes, setListaNotificacoes] = useState([]);
  const [listaCategorias, setListaCategorias] = useState([]);
  const [listaStatus, setListaStatus] = useState([]);
  const [listaTipos, setTipos] = useState([]);

  const [turmaSelecionada, setTurmaSelecionada] = useState();
  const [statusSelecionado, setStatusSelecionado] = useState();
  const [categoriaSelecionada, setCategoriaSelecionada] = useState();
  const [tipoSelecionado, setTipoSelecionado] = useState();

  const columns = [
    {
      title: 'Código',
      dataIndex: 'codigo',
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
      dataIndex: 'situacao',
    },
    {
      title: 'Data/Hora',
      dataIndex: 'dataHora',
    },
  ];

  const usuario = useSelector(store => store.usuario);

  // useEffect(() => {
  //   async function carregarListas() {
  //     const status = await api.get('v1/notificacoes/status');
  //     setListaStatus(status.data);

  //     const categorias = await api.get('v1/notificacoes/categorias');
  //     setListaCategorias(categorias.data);

  //     const tipos = await api.get('v1/notificacoes/tipos');
  //     setTipos(tipos.data);
  //   }

  //   carregarListas();
  // }, []);

  const listaSelectTurma = [
    { id: 1, descricao: 'Todas as turmas' },
    { id: 2, descricao: 'Turma selecionada' },
  ];

  function onSelectRow(row) {
    setSelectedRowKeys(row);
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

  async function onClickFiltrar() {
    // const params = {
    //   dreId: usuario.turmaSelecionada.codDre,
    //   escolaId: usuario.turmaSelecionada.codEscola,
    //   status: statusSelecionado,
    //   tipo: tipoSelecionado,
    //   turmaId: usuario.turmaSelecionada.codTurma,
    //   usuarioId: '1222', // TODO Mock
    //   categoria: categoriaSelecionada,
    //   titulo: '',
    //   codigo: '',
    //   ano: usuario.turmaSelecionada.ano,
    // };
    // const listaNotifi = await api.get('v1/notificacoes', params);

  }

  return (
    <>
      <div className="col-md-12">
        <NovoSGP>NOVO SGP</NovoSGP>
        <Titulo>Notificações</Titulo>
      </div>
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
        <div className="col-md-6 pb-2">
          <InputComponent placeholder="Pesquisar" />
        </div>

        <div className="col-md-12 ">
          <Button
            label="Excluir"
            color={Colors.Roxo}
            border
            className="mb-2 ml-2 float-right"
            onClick={() => {}}
          />
          <Button
            label="Marcar como lida"
            color={Colors.Roxo}
            border
            className="mb-2 ml-2 float-right"
            onClick={() => {}}
          />
          <Button
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
          />
          <Button
            label="Filtrar"
            color={Colors.Roxo}
            border
            className="mb-2 float-right"
            onClick={onClickFiltrar}
          />
        </div>
        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-notificacoes"
            selectedRowKeys={selectedRowKeys}
            onSelectRow={onSelectRow}
            columns={columns}
            dataSource={listaNotificacoes}
          />
        </div>
      </EstiloLista>
    </>
  );
}
