import React, { useState, useEffect } from 'react';
import { Base } from '~/componentes';
import ComponenteSemNota from './ComponenteSemNota/ComponenteSemNota';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erro } from '~/servicos/alertas';

const Sintese = props => {
  const { ehFinal, bimestreSelecionado, codigoAluno } = props;
  const cores = [
    Base.Azul,
    Base.RoxoEventoCalendario,
    Base.Laranja,
    Base.RosaCalendario,
    Base.RoxoClaro,
    Base.VerdeBorda,
    Base.Bordo,
    Base.CinzaBadge,
    Base.Preto,
    Base.VerdeBorda,
  ];

  const [dados, setDados] = useState();

  useEffect(() => {
    ServicoConselhoClasse.obterSintese(1, 1, codigoAluno)
      .then(resp => {
        setDados(resp.data);
      })
      .catch(e => {
        erro(e);
      });
  }, [bimestreSelecionado]);

  return (
    <>
      {dados
        ? dados.map((componente, i) => {
            return (
              <ComponenteSemNota
                dados={componente.componenteSinteses}
                nomeColunaComponente={componente.titulo}
                corBorda={cores[i]}
                ehFinal={ehFinal}
              />
            );
          })
        : null}
    </>
  );
};

export default Sintese;
