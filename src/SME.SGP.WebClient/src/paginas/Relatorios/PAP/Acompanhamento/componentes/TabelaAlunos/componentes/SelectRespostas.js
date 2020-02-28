import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

function SelectRespostas({
  respostas,
  objetivoAtivo,
  aluno,
  onChangeResposta,
  containerVinculoId,
  bloquearLimpar,
}) {
  const [valorPadrao, setValorPadrao] = useState(null);

  useEffect(() => {
    setValorPadrao(() => {
      const respostasAlunoFiltradaPorObjetivo = aluno.respostas.find(
        resposta => resposta.objetivoId === objetivoAtivo.id
      );

      if (!respostasAlunoFiltradaPorObjetivo) {
        if (objetivoAtivo.id === 1 || objetivoAtivo.id === 2) {
          return '2';
        }
        return null;
      }

      const respostasDisponiveisFiltradaPorSelecionada = respostas.find(
        x =>
          String(x.id) === String(respostasAlunoFiltradaPorObjetivo.respostaId)
      );

      return String(
        respostasDisponiveisFiltradaPorSelecionada
          ? respostasDisponiveisFiltradaPorSelecionada.id
          : ''
      );
    });
  }, [aluno.respostas, objetivoAtivo.id, respostas]);

  return (
    <SelectComponent
      className="fonte-14"
      onChange={valor => onChangeResposta(aluno, valor)}
      lista={respostas}
      valueSelect={valorPadrao || undefined}
      valueOption="id"
      valueText="nome"
      placeholder="Selecione a opção"
      containerVinculoId={containerVinculoId}
      allowClear={!bloquearLimpar}
    />
  );
}

SelectRespostas.propTypes = {
  respostas: t.oneOfType([t.array]),
  objetivoAtivo: t.oneOfType([t.object]),
  aluno: t.oneOfType([t.object]),
  onChangeResposta: t.func,
  containerVinculoId: t.string,
  bloquearLimpar: t.oneOfType([t.bool]),
};

SelectRespostas.defaultProps = {
  respostas: [],
  objetivoAtivo: {},
  aluno: {},
  onChangeResposta: () => null,
  containerVinculoId: '',
  bloquearLimpar: true,
};

export default SelectRespostas;
