import PropTypes from 'prop-types';
import React, { useCallback } from 'react';
import { Label } from '~/componentes';
import Editor from '~/componentes/editor/editor';
import { DescItensAutoraisProfessor } from '../../planoAnual.css';

const DescricaoPlanejamento = props => {
  const { planejamento, bimestre } = props;

  const onChange = useCallback(valorNovo => {
    console.log(valorNovo);
  }, []);

  return (
    <>
      <div className="mt-3">
        <span className="d-flex align-items-baseline">
          <Label text="Descrição do planejamento" />
          <DescItensAutoraisProfessor>
            Itens autorais do professor
          </DescItensAutoraisProfessor>
        </span>
        <Editor
          id={`bimestre-${bimestre}-editor`}
          inicial={planejamento}
          onChange={valorNovo => {
            onChange(valorNovo);
            // dispatch(setCartaIntencoesEmEdicao(true));
          }}
        />
      </div>
    </>
  );
};

DescricaoPlanejamento.propTypes = {
  planejamento: PropTypes.oneOfType([PropTypes.any]),
  bimestre: PropTypes.oneOfType([PropTypes.any]),
};

DescricaoPlanejamento.defaultProps = {
  planejamento: '',
  bimestre: '',
};

export default DescricaoPlanejamento;
