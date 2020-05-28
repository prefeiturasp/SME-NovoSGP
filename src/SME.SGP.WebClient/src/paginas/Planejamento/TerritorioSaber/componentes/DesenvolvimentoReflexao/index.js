import React, { useCallback } from 'react';
import t from 'prop-types';

// Componentes
import { Grid, Editor } from '~/componentes';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function DesenvolvimentoReflexao({ bimestre, dadosBimestre, onChange }) {
  const onChangeBimestre = useCallback(
    (bimestreAtual, campo, valor) => {
      onChange(bimestreAtual, {
        ...dadosBimestre,
        [campo]: valor,
      });
    },
    [dadosBimestre, onChange]
  );

  return (
    <Linha className="row ml-1 mr-1">
      <Grid cols={6}>
        <Editor
          onChange={valor =>
            onChangeBimestre(bimestre, 'desenvolvimento', valor)
          }
          label="Desenvolvimento das atividades"
          inicial={dadosBimestre.desenvolvimento}
        />
      </Grid>
      <Grid cols={6}>
        <Editor
          onChange={valor => onChangeBimestre(bimestre, 'reflexao', valor)}
          label="Reflexões sobre a participação dos estudantes, parcerias e avaliação"
          inicial={dadosBimestre.reflexao}
        />
      </Grid>
    </Linha>
  );
}

DesenvolvimentoReflexao.propTypes = {
  bimestre: t.oneOfType([t.any]),
  dadosBimestre: t.oneOfType([t.any]),
  onChange: t.func,
};

DesenvolvimentoReflexao.defaultProps = {
  bimestre: {},
  dadosBimestre: {},
  onChange: () => {},
};

export default DesenvolvimentoReflexao;
