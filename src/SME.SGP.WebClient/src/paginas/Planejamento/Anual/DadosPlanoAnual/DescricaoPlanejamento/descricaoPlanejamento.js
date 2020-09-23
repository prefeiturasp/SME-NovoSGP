import PropTypes from 'prop-types';
import React, { useCallback } from 'react';
import { useSelector } from 'react-redux';
import { Label } from '~/componentes';
import Editor from '~/componentes/editor/editor';
import { DescItensAutoraisProfessor } from '../../planoAnual.css';

const DescricaoPlanejamento = props => {
  const { dadosBimestre, tabAtualComponenteCurricular } = props;
  const { bimestre } = dadosBimestre;

  const dadosBimestrePlanoAnual = useSelector(
    store => store.planoAnual.dadosBimestresPlanoAnual[bimestre]
  );

  const onChange = useCallback(valorNovo => {
    console.log(valorNovo);
  }, []);

  const obterDadosComponenteAtual = () => {
    return dadosBimestrePlanoAnual?.componentes.find(
      item =>
        String(item.componenteCurricularId) ===
        String(tabAtualComponenteCurricular.codigoComponenteCurricular)
    );
  };

  return (
    <>
      {dadosBimestrePlanoAnual &&
      dadosBimestrePlanoAnual?.componentes.length &&
      obterDadosComponenteAtual() ? (
        <div className="mt-3">
          <span className="d-flex align-items-baseline">
            <Label text="Descrição do planejamento" />
            <DescItensAutoraisProfessor>
              Itens autorais do professor
            </DescItensAutoraisProfessor>
          </span>
          <Editor
            id={`bimestre-${bimestre}-editor`}
            inicial={obterDadosComponenteAtual()?.descricao}
            onChange={v => {
              if (obterDadosComponenteAtual()?.descricao !== v) {
                onChange(v);
              }
            }}
          />
        </div>
      ) : (
        ''
      )}
    </>
  );
};

DescricaoPlanejamento.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

DescricaoPlanejamento.defaultProps = {
  dadosBimestre: {},
  tabAtualComponenteCurricular: {},
};

export default DescricaoPlanejamento;
