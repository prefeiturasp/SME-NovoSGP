import React from 'react';

// Redux
import { useSelector } from 'react-redux';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, DataTable, ButtonGroup } from '~/componentes';
// import ButtonGroup from './componentes/ButtonGroup';
import Filtro from './componentes/Filtro';

function AtribuicaoEsporadicaLista() {
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const colunas = [
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
    {
      title: 'RF',
      dataIndex: 'rf',
    },
    {
      title: 'Início',
      dataIndex: 'inicio',
    },
    {
      title: 'Fim',
      dataIndex: 'fim',
    },
  ];

  const onClickVoltar = () => history.push('/');
  const onClickBotaoPrincipal = () =>
    history.push(`atribuicao-esporadica/novo`);
  return (
    <>
      <Cabecalho pagina="Atribuição esporádica" />
      <Card mx="mx-0">
        <ButtonGroup
          permissoesTela={permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]}
          temItemSelecionado
          onClickVoltar={onClickVoltar}
          onClickBotaoPrincipal={onClickBotaoPrincipal}
          labelBotaoPrincipal="Novo"
        />
        <Filtro />
        <div className="col-md-12 pt-2 py-0 px-0">
          <DataTable
            id="lista-tipo-calendario"
            // selectedRowKeys={idTiposSelecionados}
            // onSelectRow={onSelectRow}
            // onClickRow={onClickRow}
            // dataSource={listaTiposCalendarioEscolar}
            columns={colunas}
            selectMultipleRows
          />
        </div>
      </Card>
    </>
  );
}

export default AtribuicaoEsporadicaLista;
