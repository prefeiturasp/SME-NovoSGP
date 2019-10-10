import React, { useEffect, useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import DataTable from '~/componentes/table/dataTable';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';
import { confirmar, sucesso } from '~/servicos/alertas';

const TipoCalendarioEscolarLista = () => {
  const [idTiposSelecionados, setIdTiposSelecionados] = useState([]);
  const [
    listaTiposCalendarioEscolar,
    setListaTiposCalendarioEscolar,
  ] = useState([]);

  const colunas = [
    {
      title: 'Nome do tipo de calendário',
      dataIndex: 'descricaoTipoCalendario',
    },
    {
      title: 'Ano',
      dataIndex: 'ano',
    },
    {
      title: 'Período',
      dataIndex: 'periodo',
    },
  ];

  useEffect(() => {
    // TODO - Chamar endpoint
    onFiltrar();
  }, []);

  const onFiltrar = () => {
    // TODO - MOCK
    const lsitaMock = [
      {
        id: 1,
        descricaoTipoCalendario: '2019 - Calendário Escolar Educação Infantil',
        ano: '2019',
        periodo: 'Anual',
      },
      {
        id: 2,
        descricaoTipoCalendario: '2019 - Calendário Escolar',
        ano: '2019',
        periodo: 'Anual',
      },
      {
        id: 3,
        descricaoTipoCalendario: '2019 - Calendário Escolar EJA / 1º Semestre',
        ano: '2019',
        periodo: 'Semestral',
      },
      {
        id: 4,
        descricaoTipoCalendario: '2019 - Calendário Escolar EJA / 2º Semestre',
        ano: '2019',
        periodo: 'Semestral',
      },
    ];
    setListaTiposCalendarioEscolar(lsitaMock);
  };

  const onSelectRow = ids => {
    setIdTiposSelecionados(ids);
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

  const onClickEditar = id => {
    history.push(`/calendario-escolar/tipo-calendario-escolar/editar/${id}`);
  };

  const onClickExcluir = async () => {
    const confirmado = await confirmar(
      'Excluir tipo de calendário escolar',
      '',
      'Deseja realmente excluir este calendário?',
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      sucesso('Tipo de calendário excluído com sucesso.')
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
            disabled={idTiposSelecionados && idTiposSelecionados.length < 1}
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
