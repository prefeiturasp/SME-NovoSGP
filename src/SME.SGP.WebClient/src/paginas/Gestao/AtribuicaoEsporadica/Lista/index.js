import React from 'react';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, DataTable } from '~/componentes';
import ButtonGroup from './componentes/ButtonGroup';
import Filtro from './componentes/Filtro';

function AtribuicaoEsporadicaLista() {
  return (
    <>
      <Cabecalho pagina="Atribuição esporádica" />
      <Card mx="mx-0">
        <ButtonGroup />
        <Filtro />
        <div className="col-md-12 pt-2">
          <DataTable
            id="lista-tipo-calendario"
            // selectedRowKeys={idTiposSelecionados}
            // onSelectRow={onSelectRow}
            // onClickRow={onClickRow}
            // columns={colunas}
            // dataSource={listaTiposCalendarioEscolar}
            selectMultipleRows
          />
        </div>
      </Card>
    </>
  );
}

export default AtribuicaoEsporadicaLista;
