import React, { useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { Grid, Editor } from '~/componentes';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function DesenvolvimentoReflexao({ bimestre, dadosBimestre, onChange }) {
  useEffect(() => {
    console.log('Dados do bimestre', dadosBimestre);
  }, [dadosBimestre]);

  return (
    <Linha className="row ml-1 mr-1">
      <Grid cols={6}>
        <Editor label="Desenvolvimento das atividades" />
      </Grid>
      <Grid cols={6}>
        <Editor label="Reflexões sobre a participação dos estudantes, parcerias e avaliação" />
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
