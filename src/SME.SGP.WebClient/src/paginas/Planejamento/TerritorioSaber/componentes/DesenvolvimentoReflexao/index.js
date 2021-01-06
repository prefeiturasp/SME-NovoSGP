import t from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
// Componentes
import { Auditoria, Grid } from '~/componentes';
// Estilos
import { Linha } from '~/componentes/EstilosGlobais';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import RotasDto from '~/dtos/rotasDto';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

function DesenvolvimentoReflexao({ dadosBimestre, onChange }) {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.TERRITORIO_SABER];

  const [desabilitarCampos, setDesabilitarCampos] = useState(false);

  useEffect(() => {
    const somenteConsulta = verificaSomenteConsulta(permissoesTela);

    const desabilitar = !dadosBimestre.id
      ? somenteConsulta || !permissoesTela.podeIncluir
      : somenteConsulta || !permissoesTela.podeAlterar;

    setDesabilitarCampos(desabilitar);
  }, [permissoesTela, dadosBimestre]);

  const onChangeBimestre = (campo, valor) => {
    dadosBimestre[campo] = valor;
    onChange(dadosBimestre);
  };

  return (
    <>
      <Linha className="row ml-1 mr-1">
        <Grid cols={6}>
          <JoditEditor
            onChange={valor => onChangeBimestre('desenvolvimento', valor)}
            label="Desenvolvimento das atividades"
            value={dadosBimestre.desenvolvimento}
            desabilitar={desabilitarCampos}
          />
        </Grid>
        <Grid cols={6}>
          <JoditEditor
            onChange={valor => onChangeBimestre('reflexao', valor)}
            label="Reflexões sobre a participação dos estudantes, parcerias e avaliação"
            value={dadosBimestre.reflexao}
            desabilitar={desabilitarCampos}
          />
        </Grid>
      </Linha>
      <Auditoria
        criadoEm={dadosBimestre.criadoEm}
        criadoPor={dadosBimestre.criadoPor}
        criadoRf={dadosBimestre.criadoRF}
        alteradoPor={dadosBimestre.alteradoPor}
        alteradoEm={dadosBimestre.alteradoEm}
        alteradoRf={dadosBimestre.alteradoRF}
      />
    </>
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
