import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import DataTable from '~/componentes/table/dataTable';
import { URL_HOME } from '~/constantes/url';
import RotasDto from '~/dtos/rotasDto';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const TipoCalendarioEscolarLista = () => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.TIPO_CALENDARIO_ESCOLAR];

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [idTiposSelecionados, setIdTiposSelecionados] = useState([]);
  const [
    listaTiposCalendarioEscolar,
    setListaTiposCalendarioEscolar,
  ] = useState([]);

  const colunas = [
    {
      title: 'Nome do tipo de calendário',
      dataIndex: 'nome',
    },
    {
      title: 'Ano',
      dataIndex: 'anoLetivo',
    },
    {
      title: 'Período',
      dataIndex: 'descricaoPeriodo',
    },
  ];

  const onFiltrar = async () => {
    setIdTiposSelecionados([]);
    const tipos = await api.get('v1/calendarios/tipos');
    setListaTiposCalendarioEscolar(tipos.data);
  };

  useEffect(() => {
    onFiltrar();
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  const onSelectRow = ids => {
    setIdTiposSelecionados(ids);
  };

  const onClickEditar = id => {
    history.push(`/calendario-escolar/tipo-calendario-escolar/editar/${id}`);
  };

  const onClickRow = row => {
    onClickEditar(row.id);
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    history.push(`/calendario-escolar/tipo-calendario-escolar/novo`);
  };

  const onClickExcluir = async () => {
    const listaParaExcluir = [];
    idTiposSelecionados.forEach(id => {
      const tipoParaExcluir = listaTiposCalendarioEscolar.find(
        tipo => id == tipo.id
      );
      if (tipoParaExcluir) {
        listaParaExcluir.push(tipoParaExcluir);
      }
    });

    const listaNomeExcluir = listaParaExcluir.map(item => item.nome);
    const confirmado = await confirmar(
      'Excluir tipo de calendário escolar',
      listaNomeExcluir,
      `Deseja realmente excluir ${
        idTiposSelecionados.length > 1 ? 'estes calendários' : 'este calendário'
      }?`,
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const parametrosDelete = { data: idTiposSelecionados };
      const excluir = await api
        .delete('v1/calendarios/tipos', parametrosDelete)
        .catch(e => erros(e));
      if (excluir) {
        const mensagemSucesso = `${
          idTiposSelecionados.length > 1 ? 'Tipos' : 'Tipo'
        } de calendário excluído com sucesso.`;
        sucesso(mensagemSucesso);
        onFiltrar();
      }
    }
  };

  return (
    <>
      <Cabecalho pagina="Tipo de calendário escolar" />

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
              !permissoesTela.podeExcluir ||
              (idTiposSelecionados && idTiposSelecionados.length < 1)
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
            disabled={somenteConsulta || !permissoesTela.podeIncluir}
          />
        </div>

        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-tipo-calendario"
            selectedRowKeys={idTiposSelecionados}
            onSelectRow={onSelectRow}
            onClickRow={onClickRow}
            columns={colunas}
            dataSource={listaTiposCalendarioEscolar}
            selectMultipleRows
          />
        </div>
      </Card>
    </>
  );
};

export default TipoCalendarioEscolarLista;
