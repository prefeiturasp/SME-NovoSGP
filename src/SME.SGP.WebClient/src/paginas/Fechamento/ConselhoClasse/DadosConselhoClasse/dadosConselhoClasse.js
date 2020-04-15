import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import modalidadeDto from '~/dtos/modalidade';
import { setBimestreAtual } from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import AnotacoesRecomendacoes from './AnotacoesRecomendacoes/anotacoesRecomendacoes';
import servicoSalvarConselhoClasse from '../servicoSalvarConselhoClasse';

const { TabPane } = Tabs;

const DadosConselhoClasse = props => {
  const { codigoTurma, modalidade } = props;

  const dispatch = useDispatch();

  const bimestreAtual = useSelector(
    store => store.conselhoClasse.bimestreAtual
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.conselhoClasse.dadosAlunoObjectCard
  );

  const { codigoEOL, desabilitado } = dadosAlunoObjectCard;

  const setarBimestreAtual = useCallback(async () => {
    const retorno = await ServicoConselhoClasse.obterBimestreAtual(
      modalidade
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      dispatch(setBimestreAtual(String(retorno.data)));
    } else {
      dispatch(setBimestreAtual('1'));
    }
  }, [dispatch, modalidade]);

  useEffect(() => {
    if (codigoEOL && !bimestreAtual.valor) {
      setarBimestreAtual();
    }
  }, [codigoEOL, bimestreAtual, setarBimestreAtual]);

  const onChangeTab = async numeroBimestre => {
    const continuar = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
    if (continuar) {
      dispatch(setBimestreAtual(numeroBimestre));
    }
  };

  const montarDadosAnotacoesRecomendacoes = () => {
    return (
      <>
        <AnotacoesRecomendacoes
          bimestreSelecionado={bimestreAtual}
          codigoTurma={codigoTurma}
          modalidade={modalidade}
          codigoEOL={codigoEOL}
          alunoDesabilitado={desabilitado}
        />
      </>
    );
  };

  return (
    <>
      {codigoEOL && bimestreAtual.valor ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={bimestreAtual.valor}
          className={
            modalidade === modalidadeDto.EJA
              ? 'ant-tab-nav-33'
              : 'ant-tab-nav-20'
          }
        >
          <TabPane tab="1º Bimestre" key="1">
            {bimestreAtual.valor === '1'
              ? montarDadosAnotacoesRecomendacoes()
              : ''}
          </TabPane>
          <TabPane tab="2º Bimestre" key="2">
            {bimestreAtual.valor === '2'
              ? montarDadosAnotacoesRecomendacoes()
              : ''}
          </TabPane>
          <TabPane tab="3º Bimestre" key="3">
            {bimestreAtual.valor === '3'
              ? montarDadosAnotacoesRecomendacoes()
              : ''}
          </TabPane>
          <TabPane tab="4º Bimestre" key="4">
            {bimestreAtual.valor === '4'
              ? montarDadosAnotacoesRecomendacoes()
              : ''}
          </TabPane>
          <TabPane tab="Final" key="final">
            {bimestreAtual.valor === 'final'
              ? montarDadosAnotacoesRecomendacoes()
              : ''}
          </TabPane>
        </ContainerTabsCard>
      ) : (
        ''
      )}
    </>
  );
};

DadosConselhoClasse.propTypes = {
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  modalidade: PropTypes.oneOfType([PropTypes.any]),
};

DadosConselhoClasse.defaultProps = {
  codigoTurma: '',
  modalidade: '',
};

export default DadosConselhoClasse;
