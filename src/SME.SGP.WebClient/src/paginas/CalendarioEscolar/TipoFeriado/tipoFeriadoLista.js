import React, { useEffect, useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import { URL_HOME } from '~/constantes/url';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

const TipoFeriadoLista = () => {
  const [idsTipoFeriadoSelecionado, setIdsTipoFeriadoSelecionado] = useState(
    []
  );
  const [listaTipoFeriado, setListaTipoFeriado] = useState([]);
  const [nomeTipoFeriado, setNomeTipoFeriado] = useState('');
  const [
    dropdownAbrangenciaSelecionada,
    setDropdownAbrangenciaSelecionada,
  ] = useState(0);
  const [
    dropdownTipoFeriadoSelecionado,
    setDropdownTipoFeriadoSelecionado,
  ] = useState(0);

  const listaDropdownAbrangencia = [
    { id: 1, nome: 'Nacional' },
    { id: 2, nome: 'Estadual' },
    { id: 3, nome: 'Municipal' },
  ];

  const listaDropdownTipoFeriado = [
    { id: 1, nome: 'Fixo' },
    { id: 2, nome: 'Móvel' },
  ];

  const colunas = [
    {
      title: 'Nome do Feriado',
      dataIndex: 'nome',
    },
    {
      title: 'Abrangência',
      dataIndex: 'descricaoAbrangencia',
    },
    {
      title: 'Tipo',
      dataIndex: 'descricaoTipo',
    },
  ];

  useEffect(() => {
    onFiltrar();
  }, [nomeTipoFeriado, dropdownAbrangenciaSelecionada, dropdownTipoFeriadoSelecionado]);

  const onFiltrar = async () => {
    setIdsTipoFeriadoSelecionado([]);
    const parametros = {
      nome: nomeTipoFeriado,
      abrangencia: dropdownAbrangenciaSelecionada || 0,
      tipo: dropdownTipoFeriadoSelecionado || 0
    };
    const tipos = await api.post('v1/calendarios/feriados/listar', parametros);
    setListaTipoFeriado(tipos.data);
  };

  const onSelectRow = ids => {
    setIdsTipoFeriadoSelecionado(ids);
  };

  const onClickRow = row => {
    onClickEditar(row.id);
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    history.push(`/calendario-escolar/tipo-feriado/novo`);
  };

  const onClickEditar = id => {
    history.push(`/calendario-escolar/tipo-feriado/editar/${id}`);
  };

  const onClickExcluir = async () => {
    const listaParaExcluir = [];
    idsTipoFeriadoSelecionado.forEach(id => {
      const achou = listaTipoFeriado.find(tipo => id == tipo.id);
      if (achou) {
        listaParaExcluir.push(achou);
      }
    });

    const listaNomeExcluir = listaParaExcluir.map(item => item.nome);
    const confirmado = await confirmar(
      'Excluir tipo feriado',
      listaNomeExcluir,
      `Deseja realmente excluir ${
        idsTipoFeriadoSelecionado.length > 1 ? 'estes feriados' : 'este feriado'
      }?`,
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const parametrosDelete = { data: idsTipoFeriadoSelecionado };
      const excluir = await api
        .delete('v1/calendarios/feriados', parametrosDelete)
        .catch(e => erros(e));
      if (excluir && excluir.status == 200) {
        const mensagemSucesso = `${
          idsTipoFeriadoSelecionado.length > 1 ? 'Tipos' : 'Tipo'
        } de feriado excluído com sucesso.`;
        sucesso(mensagemSucesso);
        onFiltrar();
      }
    }
  };

  const onChangeDropdownAbrangencia = abrangencia => {
    setDropdownAbrangenciaSelecionada(abrangencia);
  };

  const onChangeDropdownTipoFeriado = tipo => {
    setDropdownTipoFeriadoSelecionado(tipo);
  };

  const onChangeNomeTipoFeriado = e => {
    setNomeTipoFeriado(e.target.value);
  };

  return (
    <>
      <Cabecalho pagina="Lista de tipo de feriado" />
      <Card>
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
            disabled={
              idsTipoFeriadoSelecionado && idsTipoFeriadoSelecionado.length < 1
            }
            onClick={onClickExcluir}
          />
          <Button
            label="Novo"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickNovo}
          />
        </div>

        <div className="col-md-4 pb-2">
          <SelectComponent
            label="Abrangência"
            name="select-abrangencia"
            id="select-abrangencia"
            lista={listaDropdownAbrangencia}
            valueOption="id"
            valueText="nome"
            onChange={onChangeDropdownAbrangencia}
            valueSelect={dropdownAbrangenciaSelecionada || []}
            placeholder="SELECIONE UMA ABRANGÊNCIA"
          />
        </div>
        <div className="col-md-3 pb-2">
          <SelectComponent
            label="Tipo"
            name="select-tipo-feiado"
            id="select-tipo-friado"
            lista={listaDropdownTipoFeriado}
            valueOption="id"
            valueText="nome"
            onChange={onChangeDropdownTipoFeriado}
            valueSelect={dropdownTipoFeriadoSelecionado || []}
            placeholder="SELECIONE UM TIPO"
          />
        </div>

        <div className="col-md-5 pb-2">
          <CampoTexto
            label="Nome do tipo de feriado"
            placeholder="DIGITE O NOME DO TIPO DE FERIADO"
            onChange={onChangeNomeTipoFeriado}
            value={nomeTipoFeriado}
          />
        </div>

        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-tipo-calendario"
            selectedRowKeys={idsTipoFeriadoSelecionado}
            onSelectRow={onSelectRow}
            onClickRow={onClickRow}
            columns={colunas}
            dataSource={listaTipoFeriado}
            selectMultipleRows
          />
        </div>
      </Card>
    </>
  );
};

export default TipoFeriadoLista;
