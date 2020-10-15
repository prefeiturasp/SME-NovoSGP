import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import TransferenciaLista from '~/componentes-sgp/TranferenciaLista/transferenciaLista';
import {
  setExibirLoaderFrequenciaPlanoAula,
  setModoEdicaoPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros } from '~/servicos/alertas';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';
import { ContainerListaObjetivos } from './listaObjetivosPlanoAula.css';

const ListaObjetivosPlanoAula = React.memo(props => {
  const { tabAtualComponenteCurricular } = props;

  const dispatch = useDispatch();

  const temPeriodoAberto = useSelector(
    state => state.frequenciaPlanoAula.listaDadosFrequencia?.temPeriodoAberto
  );

  const dadosParaSalvarPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.dadosParaSalvarPlanoAula
  );

  const [dadosEsquerda, setDadosEsquerda] = useState([]);
  const [dadosDireita, setDadosDireita] = useState([]);
  const [idsSelecionadsEsquerda, setIdsSelecionadsEsquerda] = useState([]);
  const [idsSelecionadsDireita, setIdsSelecionadsDireita] = useState([]);

  const montarDadosListasObjetivos = useCallback(
    listaObjetivos => {
      if (
        dadosParaSalvarPlanoAula.objetivosAprendizagemAula &&
        dadosParaSalvarPlanoAula.objetivosAprendizagemAula.length
      ) {
        const listaEsquerda = [];
        const listaDireita = [];
        listaObjetivos.forEach(objetivo => {
          if (
            dadosParaSalvarPlanoAula.objetivosAprendizagemAula.find(
              item => item.id === objetivo.id
            )
          ) {
            listaDireita.push(objetivo);
          } else {
            listaEsquerda.push(objetivo);
          }
        });

        if (listaEsquerda.length) {
          setDadosEsquerda(listaEsquerda);
        }
        if (listaDireita.length) {
          setDadosDireita(listaDireita);
        }
      } else {
        setDadosEsquerda(listaObjetivos);
      }
    },
    [dadosParaSalvarPlanoAula]
  );

  // TODO Verificar para salvar dados no redux e não consultar no banco novamente a cada mudança de tab!
  const obterObjetivosPorAnoEComponenteCurricular = useCallback(() => {
    if (
      tabAtualComponenteCurricular &&
      tabAtualComponenteCurricular.codigoComponenteCurricular
    ) {
      dispatch(setExibirLoaderFrequenciaPlanoAula(true));
      ServicoPlanoAula.obterListaObjetivosPorAnoEComponenteCurricular(
        tabAtualComponenteCurricular.codigoComponenteCurricular
      )
        .then(listaObjetivos => {
          montarDadosListasObjetivos(listaObjetivos);
        })
        .catch(e => {
          erros(e);
        })
        .finally(() => {
          dispatch(setExibirLoaderFrequenciaPlanoAula(false));
        });
    }
  }, [dispatch, tabAtualComponenteCurricular, montarDadosListasObjetivos]);

  useEffect(() => {
    if (tabAtualComponenteCurricular) {
      obterObjetivosPorAnoEComponenteCurricular();
    }
  }, [tabAtualComponenteCurricular, obterObjetivosPorAnoEComponenteCurricular]);

  const parametrosListaEsquerda = {
    title: 'Objetivos de aprendizagem',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosEsquerda,
    onSelectRow: setIdsSelecionadsEsquerda,
    selectedRowKeys: idsSelecionadsEsquerda,
    selectMultipleRows: temPeriodoAberto,
  };

  const parametrosListaDireita = {
    title:
      dadosDireita && dadosDireita.length
        ? 'Objetivos de aprendizagem selecionados para este componente curricular'
        : 'Você precisa selecionar ao menos um objetivo para poder inserir a descrição do plano',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo fundo-codigo-lista-direita',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosDireita,
    onSelectRow: setIdsSelecionadsDireita,
    selectedRowKeys: idsSelecionadsDireita,
    selectMultipleRows: temPeriodoAberto,
  };

  const obterListaComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => String(id) === String(item.id)));
  };

  const obterListaSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => String(id) === String(item.id)));
  };

  const onClickAdicionar = () => {
    if (idsSelecionadsEsquerda && idsSelecionadsEsquerda.length) {
      const novaListaDireita = obterListaComIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      const novaListaEsquerda = obterListaSemIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      setDadosEsquerda([...novaListaEsquerda]);
      setDadosDireita([...novaListaDireita, ...dadosDireita]);

      const novaLista = [...novaListaDireita, ...dadosDireita];
      ServicoPlanoAula.atualizarDadosParaSalvarPlanoAula(
        'objetivosAprendizagemAula',
        novaLista
      );

      dispatch(setModoEdicaoPlanoAula(true));
      setIdsSelecionadsEsquerda([]);
    }
  };

  const onClickRemover = async () => {
    if (idsSelecionadsDireita && idsSelecionadsDireita.length) {
      const novaListaEsquerda = obterListaComIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      const novaListaDireita = obterListaSemIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      setDadosEsquerda([...novaListaEsquerda, ...dadosEsquerda]);
      setDadosDireita([...novaListaDireita]);

      const novaLista = [...novaListaDireita];
      ServicoPlanoAula.atualizarDadosParaSalvarPlanoAula(
        'objetivosAprendizagemAula',
        novaLista
      );

      dispatch(setModoEdicaoPlanoAula(true));
      setIdsSelecionadsDireita([]);
    }
  };

  return (
    <ContainerListaObjetivos>
      <TransferenciaLista
        listaEsquerda={parametrosListaEsquerda}
        listaDireita={parametrosListaDireita}
        onClickAdicionar={onClickAdicionar}
        onClickRemover={onClickRemover}
      />
    </ContainerListaObjetivos>
  );
});

ListaObjetivosPlanoAula.propTypes = {
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

ListaObjetivosPlanoAula.defaultProps = {
  tabAtualComponenteCurricular: {},
};

export default ListaObjetivosPlanoAula;
