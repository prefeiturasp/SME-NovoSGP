import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Base } from '~/componentes';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import ComponenteSemNota from './ComponenteSemNota/ComponenteSemNota';

const Sintese = props => {
  const { ehFinal, turmaId, bimestreSelecionado } = props;

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  const {
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
  } = dadosPrincipaisConselhoClasse;

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

  const [dados, setDados] = useState([]);

  useEffect(() => {
    if (turmaId && bimestreSelecionado && alunoCodigo) {
      ServicoConselhoClasse.obterSintese(
        conselhoClasseId,
        fechamentoTurmaId,
        alunoCodigo,
        turmaId,
        bimestreSelecionado
      )
        .then(resp => {
          setDados(resp.data);
        })
        .catch(e => {
          erros(e);
          setDados([]);
        });
    }
  }, [
    alunoCodigo,
    conselhoClasseId,
    fechamentoTurmaId,
    turmaId,
    bimestreSelecionado,
  ]);

  return (
    <>
      {dados && dados.length
        ? dados.map((componente, i) => {
            return (
              <div className="pl-2 pr-2" key={shortid.generate()}>
                <ComponenteSemNota
                  dados={componente.componenteSinteses}
                  nomeColunaComponente={componente.titulo}
                  corBorda={cores[i]}
                  ehFinal={ehFinal}
                />
              </div>
            );
          })
        : null}
    </>
  );
};

Sintese.propTypes = {
  ehFinal: PropTypes.oneOfType([PropTypes.bool]),
  turmaId: PropTypes.oneOfType([PropTypes.number]),
  bimestreSelecionado: PropTypes.oneOfType([PropTypes.number]),
};

Sintese.defaultProps = {
  ehFinal: false,
  turmaId: 0,
  bimestreSelecionado: 0,
};

export default Sintese;
