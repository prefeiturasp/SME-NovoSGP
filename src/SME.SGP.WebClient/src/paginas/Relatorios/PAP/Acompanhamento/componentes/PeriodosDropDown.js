import React, { useEffect, useCallback, useState } from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';
import AcompanhamentoPAPServico from '~/servicos/Paginas/Relatorios/PAP/Acompanhamento';

function PeriodosDropDown({
  onChangePeriodo,
  valor,
  desabilitado,
  codigoTurma,
  setSemPeriodos,
}) {
  const [opcoes, setOpcoes] = useState(null);

  const obterOpcoes = useCallback(async () => {
    if (!codigoTurma || codigoTurma === 0 || codigoTurma === '0') return;

    const retorno = await AcompanhamentoPAPServico.ObterPeriodos(codigoTurma);

    if (!retorno.sucesso || retorno.semDados) {
      setSemPeriodos(true);
      return;
    }

    setSemPeriodos(false);
    setOpcoes(
      retorno.dados.map(x => {
        return { valor: x.id, descricao: x.nome };
      })
    );
  }, [codigoTurma, setSemPeriodos]);

  useEffect(() => {
    obterOpcoes();
  }, [obterOpcoes, codigoTurma]);

  return (
    <SelectComponent
      onChange={onChangePeriodo}
      valueOption="valor"
      valueText="descricao"
      lista={opcoes}
      placeholder="Selecione o período"
      valueSelect={valor}
      disabled={desabilitado}
    />
  );
}

PeriodosDropDown.propTypes = {
  onChangePeriodo: t.func,
  valor: t.string,
  desabilitado: t.bool,
  codigoTurma: t.string,
  setSemPeriodos: t.func,
};

PeriodosDropDown.defaultProps = {
  onChangePeriodo: () => {},
  valor: undefined,
  desabilitado: false,
  codigoTurma: '',
  setSemPeriodos: () => {},
};

export default PeriodosDropDown;
