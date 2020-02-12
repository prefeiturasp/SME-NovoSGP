import React from 'react';
import moment from 'moment';
import { InputNumber } from 'antd';
import { Label, SelectComponent } from '~/componentes';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import { erros } from '~/servicos/alertas';

export default function NotaRegencia({
  aluno,
  ehNota,
  listaConceitos,
  onChange,
}) {
  const onChangeNota = (alunoSelecionado, nota, notaConceito) => {
    debugger;
    ServicoNotaConceito.obterArredondamento(
      notaConceito,
      moment().format('YYYY-MM-DD')
    )
      .then(resposta => {
        nota.notaConceito = resposta.data;
        onChange(alunoSelecionado, resposta.data, nota.disciplinaCodigo);
      })
      .catch(e => erros(e));
  };
  return (
    <div className="coluna-regencia">
      {aluno.notasConceitoFinal.map(nota =>
        ehNota ? (
          <div className="input-regencia">
            <Label text={nota.disciplina} className="break-word" />
            <InputNumber
              classNameCampo="col-conceito-regencia"
              min={1}
              max={10}
              defaultValue={nota.notaConceito}
              onChange={value => onChangeNota(aluno, nota, value)}
            />
          </div>
        ) : (
          <SelectComponent
            id={`conceitosNota-${nota.disciplina}-${aluno.numeroChamada}`}
            name={`conceitosNota-${nota.disciplina}-${aluno.numeroChamada}`}
            label={nota.disciplina}
            lista={listaConceitos}
            valueOption="id"
            valueText="valor"
            placeholder="Selecione um conceito"
            classNameContainer="select-conceito"
            valueSelect={nota.notaConceito}
            onChange={conceito => {
              nota.notaConceito = conceito;
              onChange(aluno, nota.notaConceito, nota.disciplinaCodigo);
            }}
          />
        )
      )}
    </div>
  );
}
