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
import { Card, ListaPaginada, ButtonGroup, Loader } from '~/componentes';
import Filtro from './componentes/Filtro';

function AtribuicaoCJLista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const colunas = [
    {
      title: 'Modalidade',
      dataIndex: 'modalidade',
    },
    {
      title: 'Turma',
      dataIndex: 'turma',
    },
    {
      title: 'Disciplina',
      dataIndex: 'disciplina',
    },
  ];

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () => {
    history.push(`atribuicao-cjs/novo`);
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
    history.push(`/gestao/atribuicao-cjs/editar/${item.id}`);
  };

  const onChangeFiltro = valoresFiltro => {
    setFiltro({
      AnoLetivo: '2019',
      DreId: valoresFiltro.dreId,
      UeId: valoresFiltro.ueId,
      ProfessorRF: valoresFiltro.professorRf,
    });
  };

  const validarFiltro = () => {
    debugger;
    return !!filtro.DreId && !!filtro.UeId;
  };

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  return (
    <>
      <Cabecalho pagina="Atribuição de CJ" />
      <Loader loading={false}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={
              permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]
            }
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
              url="v1/atribuicoes/cjs"
              id="lista-atribuicoes-esporadica"
              colunaChave="id"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
              filtroEhValido={validarFiltro()}
            />
          </div>
        </Card>
      </Loader>
    </>
  );
}

export default AtribuicaoCJLista;
