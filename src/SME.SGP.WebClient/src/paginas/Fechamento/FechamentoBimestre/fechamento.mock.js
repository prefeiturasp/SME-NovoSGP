export const FechamentoMock = () => {
  return {
    totalAulasPrevistas: 343,
    totalAulasDadas: 300,
    lista: [
      {
        contador: 1,
        informacao: undefined,
        nome: 'Emerson Ramos Grassi',
        faltas_bimestre: 12,
        ausencias_compensadas: 12,
        frequencia: 70,
        ativo: true,
        regencia: [
          {
            disciplina: 'Português',
            nota: 2,
          },
          {
            disciplina: 'Matemática',
            nota: 3,
          },
          {
            disciplina: 'História',
            nota: 4,
          },
        ],
      },
      {
        contador: 2,
        informacao: 'Estudante transferido em 01/02/2020',
        nome: 'Alvaro Ramos Grassi',
        nota_conceito: 8.5,
        faltas_bimestre: 12,
        ausencias_compensadas: 12,
        frequencia: 70,
        ativo: true,
      },
      {
        contador: 3,
        nome: 'Aline  Grassi',
        nota_conceito: 9,
        faltas_bimestre: 3,
        ausencias_compensadas: 3,
        frequencia: 89,
        ativo: true,
      },
      {
        contador: 4,
        nome: 'Valentina  Grassi',
        nota_conceito: undefined,
        faltas_bimestre: undefined,
        ausencias_compensadas: undefined,
        frequencia: undefined,
        ativo: false,
        informacao: 'desabilitado',
      },
      {
        contador: 5,
        informacao: undefined,
        nome: 'Cleiton Ramos Grassi',
        faltas_bimestre: 12,
        ausencias_compensadas: 12,
        frequencia: 70,
        ativo: true,
        regencia: [
          {
            disciplina: 'Português',
            nota: 2,
          },
          {
            disciplina: 'Matemática',
            nota: 3,
          },
          {
            disciplina: 'História',
            nota: 4,
          },
        ],
      },
    ],
  }
}
