import React, { useState } from 'react';

import { useSelector } from 'react-redux';

import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';

import Filtro from './componentes/Filtro';
import ServicoBoletimSimples from '~/servicos/Paginas/Relatorios/DiarioClasse/BoletimSimples/ServicoBoletimSimples';

const BoletimSimples = () => {
  const [loaderSecao] = useState(false);
  const [somenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const [itensSelecionados, setItensSelecionados] = useState([]);

  const onSelecionarItems = items => {
    setItensSelecionados([...items.map(item => String(item.id))]);
  };

  const [filtro, setFiltro] = useState({});

  const onChangeFiltro = valoresFiltro => {
    setFiltro(valoresFiltro);
  };

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickBotaoPrincipal = () => {
    ServicoBoletimSimples.imprimirBoletim({
      ...filtro,
      alunosId: itensSelecionados,
    });
  };

  const colunas = [
    {
      title: 'Número',
      dataIndex: 'numero',
    },
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
  ];

  return (
    <>
      <Cabecalho pagina="Impressão de Boletim" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.RELATORIO_BOLETIM_SIMPLES]}
            temItemSelecionado={itensSelecionados && itensSelecionados.length}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Imprimir"
          />
          <Filtro onFiltrar={onChangeFiltro} />
          {filtro && filtro.opcaoAlunoId === '1' ? (
            <div className="col-md-12 pt-2 py-0 px-0">
              <ListaPaginada
                id="lista-alunos"
                url="v1/boletim/alunos"
                idLinha="id"
                colunaChave="id"
                colunas={colunas}
                multiSelecao
                filtro={filtro}
                paramArrayFormat="repeat"
                selecionarItems={onSelecionarItems}
                filtroEhValido
              />
            </div>
          ) : null}
        </Card>
      </Loader>
    </>
  );
};

export default BoletimSimples;
