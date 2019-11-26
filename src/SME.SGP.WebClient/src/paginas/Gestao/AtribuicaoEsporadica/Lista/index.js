import React, { useState, useEffect } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { confirmar, sucesso } from '~/servicos/alertas';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, ListaPaginada, ButtonGroup } from '~/componentes';
import Filtro from './componentes/Filtro';

function AtribuicaoEsporadicaLista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [refForm] = useState({});
  const [somenteConsulta, setSomenteConsulta] = useState(false);
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
        const excluir = await Promise.all(
          itensSelecionados.map(x =>
            AtribuicaoEsporadicaServico.deletarAtribuicaoEsporadica(x.id)
          )
        );
        if (excluir) {
          const mensagemSucesso = `${
            itensSelecionados.length > 1
              ? 'Eventos excluídos'
              : 'Evento excluído'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          setFiltro({
            ...filtro,
            atualizar: !filtro.atualizar || true,
          });
          setItensSelecionados([]);
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

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  return (
    <>
      <Cabecalho pagina="Atribuição esporádica" />
      <Card mx="mx-0">
        <ButtonGroup
          somenteConsulta={somenteConsulta}
          permissoesTela={permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]}
          temItemSelecionado={
            itensSelecionados && itensSelecionados.length >= 1
          }
          onClickVoltar={onClickVoltar}
          onClickExcluir={onClickExcluir}
          onClickBotaoPrincipal={onClickBotaoPrincipal}
          labelBotaoPrincipal="Novo"
          desabilitarBotaoPrincipal={
            !!filtro.dreId === false && !!filtro.ueId === false
          }
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
