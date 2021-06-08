pipeline {
    agent {
      node { 
        label 'dockerdotnet2'
      }
    }
    
    options {
      buildDiscarder(logRotator(numToKeepStr: '50', artifactNumToKeepStr: '50'))
      disableConcurrentBuilds()
      skipDefaultCheckout()  
    }
    
        
    stages {
      stage('CheckOut') {
        steps {
          checkout scm  
        }
       }
       
      stage('Build projeto') {
            steps {
            sh "echo executando build de projeto"
            sh 'dotnet build'
            }
        }
        
      stage('Testes API DEV') {
       

           steps {
             withCredentials([file(credentialsId: 'dev-newman-sgp', variable: 'NEWMANSGPDEV')]) {
                 
               sh 'pwd'
               sh 'who'
               sh 'cp $NEWMANSGPDEV teste/Postman/Dev.json'
               sh 'newman run teste/Postman/GradeComponentesCurriculares.json -e teste/Postman/Dev.json -r htmlextra --reporter-htmlextra-titleSize 4 --reporter-htmlextra-title "Grade dos Componentes Curriculares" --reporter-htmlextra-export ./results/reportgcc.html'
               publishHTML([allowMissing: false, alwaysLinkToLastBuild: false, keepAll: false, reportDir: 'results', reportFiles: 'reportgcc.html', reportName: 'Postman Report', reportTitles: 'Report'])
             
             } 
           }
      }

      stage('Testes API HOM') {
        when {
           branch 'release'
        }
        options { retry(3) }

           steps {
             withCredentials([file(credentialsId: 'hom-newman-sgp', variable: 'NEWMANSGPHOM')]) {
               sh 'cp $NEWMANSGPHOM teste/Postman/Hom.json'
               sh 'newman run teste/Postman/GradeComponentesCurriculares.json -e teste/Postman/Hom.json -r htmlextra --reporter-htmlextra-titleSize 4 --reporter-htmlextra-title "Grade dos Componentes Curriculares" --reporter-htmlextra-export ./results/report.html'
               publishHTML([allowMissing: false, alwaysLinkToLastBuild: false, keepAll: false, reportDir: 'results', reportFiles: 'report.html', reportName: 'Postman Report', reportTitles: 'Report'])
             
             } 
           }
      }

      stage('Docker build DEV') {
        when {
          branch 'development'
        }
          steps {
            sh 'echo Build Docker image'
                
        // Start JOB Rundeck para build das imagens Docker e push SME Registry
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                               
              //JOB DE BUILD
              jobId: "743ccbae-bd30-4ac6-b2a3-2f0d1c64e937",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
          }
      }

      stage('Deploy DEV') {
        when {
          branch 'development'
        }
          steps {
            sh 'echo Deploying desenvolvimento'            
       //Start JOB Rundeck para update de deploy Kubernetes DEV
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "f6c3e74c-6411-466a-84a7-921d637c2645",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }
        
      stage('Docker build HOM') {
            when {
              branch 'release'
            }
            steps {
              sh 'echo Deploying homologacao'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                
               
              //JOB DE BUILD
              jobId: "397ce3f8-0af7-4d26-b65b-19f09ccf6c82",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
            } 
        }
      
      stage('Deploy HOM') {
            when {
                branch 'release'
            }
            steps {
                 timeout(time: 24, unit: "HOURS") {
               
                 telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
                 input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marlon_goncalves, luiz_araujo, marcos_costa, bruno_alevato, robson_silva, rafael_losi'
            }
                 sh 'echo Deploying homologacao'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                
               
              //JOB DE BUILD
              jobId: "397ce3f8-0af7-4d26-b65b-19f09ccf6c82",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para update de imagens no host homologação 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "124f1ff0-d903-40fd-9455-fc91907293a7",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }
        
        stage('Docker build HOM-R2') {
            when {
              branch 'release-r2'
            }
            steps {
              sh 'echo Deploying homologacao'
                
        // Start JOB Rundeck para build das imagens Docker
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                
               
              //JOB DE BUILD
              jobId: "e15cd478-1155-40a2-842c-11d1de0512eb",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
            }
        }

      stage('Deploy HOM-R2') {
            when {
              branch 'release-r2'
            }
            steps {     
                
       //Start JOB Rundeck para update de imagens no host homologação 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "b45bb11d-889a-4783-b70d-4406ea6817d7",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }    


        stage('Docker Build PROD') {

            when {
              branch 'master'
            }
            steps {
                 timeout(time: 24, unit: "HOURS") {
               
                 telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
                 input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marlon_goncalves, luiz_araujo, marcos_costa, bruno_alevato, robson_silva, rafael_losi'
            }
                 sh 'echo Deploy produção'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
            
               
              //JOB DE BUILD
              jobId: "b6ff0cbf-6267-41af-bb56-5cdc3eb86902",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
            }
        }

      stage('Deploy PROD') {
          when {
            branch 'master'
          }
          steps {
            timeout(time: 24, unit: "HOURS") {
              telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
              input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marlon_goncalves, allan_santos, everton_nogueira, marcos_costa, bruno_alevato, robson_silva, rafael_losi'
            }       
                
       //Start JOB Rundeck para deploy em produção 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "6a3d314b-672b-4fe3-9759-0b08847eb27e",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
         }
        }
      }
     
}

}
