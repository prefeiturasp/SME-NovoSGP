import React, { useState, useEffect } from 'react';

// Componentes
import { SelectComponent } from '~/componentes';

function SelectRespostas({
  respostas,
  objetivoAtivo,
  aluno,
  onChangeResposta,
}) {
  const [valorPadrao, setValorPadrao] = useState(null);

  useEffect(() => {
    setValorPadrao(() => {
      const respostasAlunoFiltradaPorObjetivo = aluno.respostas.find(
        y => y.objetivoId === objetivoAtivo.id
      );

      if (!respostasAlunoFiltradaPorObjetivo) {
        return null;
      }

      const respostasDisponiveisFiltradaPorSelecionada = respostas.find(
        x =>
          String(x.id) === String(respostasAlunoFiltradaPorObjetivo.respostaId)
      );

      return String(respostasDisponiveisFiltradaPorSelecionada.id);
    });
  }, [
    aluno.Respostas,
    aluno.respostas,
    objetivoAtivo.Id,
    objetivoAtivo.id,
    respostas,
    respostas.Opcoes,
  ]);

  return (
    <SelectComponent
      className="fonte-14"
      onChange={valor => onChangeResposta(aluno, valor)}
      lista={respostas}
      valueSelect={valorPadrao || undefined}
      valueOption="id"
      valueText="descricao"
      placeholder="Selecione a opção"
    />
  );
}

export default SelectRespostas;
