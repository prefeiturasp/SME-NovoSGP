import React, { useState } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { confirmar, sucesso } from '~/servicos/alertas';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, ListaPaginada, ButtonGroup } from '~/componentes';
import Filtro from './componentes/Filtro';

function AtribuicaoEsporadicaLista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = window.moment(data).format('DD/MM/YYYY');
    }
    return <span>{dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Nome',
      dataIndex: 'professorNome',
    },
    {
      title: 'RF',
      dataIndex: 'professorRf',
    },
    {
      title: 'Início',
      dataIndex: 'dataInicio',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Fim',
      dataIndex: 'dataFim',
      render: data => formatarCampoDataGrid(data),
    },
  ];

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () => {
    history.push(`atribuicao-esporadica/novo`);
  };

  const onSelecionarItems = items => {
    setItensSelecionados(items);
  };

  const onClickExcluir = async () => {
    if (itensSelecionados && itensSelecionados.length > 0) {
      const listaNomeExcluir = itensSelecionados.map(
        item => item.professorNome
      );
      const confirmado = await confirmar(
        'Excluir atribuição',
        listaNomeExcluir,
        `Deseja realmente excluir ${
          itensSelecionados.length > 1 ? 'estes itens' : 'este item'
        }?`,
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        // const idsDeletar = itensSelecionados.map(c => c.id);
        // const excluir = await servicoEvento.deletar(idsDeletar);
        // TODO: Integrar metodo deletar
        const excluir = await Promise.resolve({ result: true, status: 200 });
        if (excluir && excluir.status === 200) {
          const mensagemSucesso = `${
            itensSelecionados.length > 1
              ? 'Eventos excluídos'
              : 'Evento excluído'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          // validaFiltrar();
        }
      }
    }
  };

  const onClickEditar = item => {
    history.push(`/gestao/atribuicao-esporadica/editar/${item.id}`);
  };

  const onChangeFiltro = valoresFiltro => {
    setFiltro(valoresFiltro);
  };

  return (
    <>
      <Cabecalho pagina="Atribuição esporádica" />
      <Card mx="mx-0">
        <ButtonGroup
          permissoesTela={permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]}
          temItemSelecionado={
            itensSelecionados && itensSelecionados.length >= 1
          }
          onClickVoltar={onClickVoltar}
          onClickExcluir={onClickExcluir}
          onClickBotaoPrincipal={onClickBotaoPrincipal}
          labelBotaoPrincipal="Novo"
        />
        <Filtro onFiltrar={onChangeFiltro} />
        <div className="col-md-12 pt-2 py-0 px-0">
          <ListaPaginada
            url="v1/atribuicao/esporadica/listar"
            id="lista-atribuicoes-esporadica"
            colunaChave="id"
            colunas={colunas}
            filtro={filtro}
            onClick={onClickEditar}
            multiSelecao
            selecionarItems={onSelecionarItems}
          />
        </div>
      </Card>
    </>
  );
}

export default AtribuicaoEsporadicaLista;
