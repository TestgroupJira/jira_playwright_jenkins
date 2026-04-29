pipeline {
    agent any
    
    parameters {
        string(name: 'FILTER', defaultValue: '', description: 'Filtro de tests enviado desde GitHub')
        string(name: 'JIRA_TICKET_ID', defaultValue: '', description: 'ID dinámico del ticket de Jira')
    }

    environment {
        CURL_EXE = 'C:\\Windows\\System32\\curl.exe'
        GITHUB_TOKEN = credentials('github-token')
    }

    stages {
        stage('Preparar Entorno') {
            steps {
                bat 'dotnet restore SauceDemoLogin.sln'
                bat 'dotnet build SauceDemoLogin.sln --configuration Release --no-restore'
                bat 'dotnet new tool-manifest --force'
                bat 'dotnet tool install Microsoft.Playwright.CLI || echo Ya instalada'
                bat 'dotnet tool run playwright install chromium'
            }
        }
        
        stage('Ejecutar Pruebas Automatizadas') {
            steps {
                script {
                    try {
                        def filterArgs = params.FILTER ? "--filter \"${params.FILTER}\"" : ""
                        echo "Lanzando Playwright para el Ticket ${params.JIRA_TICKET_ID} con filtro: ${filterArgs}"
                        
                        bat "dotnet test SauceDemoLogin.sln --configuration Release ${filterArgs} --logger:trx --results-directory ./TestResults"
                        
                        currentBuild.result = 'SUCCESS'
                    } catch (Exception e) {
                        currentBuild.result = 'FAILURE'
                        echo "Atención: Las pruebas fallaron para el ticket ${params.JIRA_TICKET_ID}."
                    }
                }
            }
        }
    }
    
    post {
        always {
            script {
                withCredentials([string(credentialsId: 'github-token', variable: 'TOKEN')]) {
                    def bStatus = currentBuild.result ?: 'SUCCESS'
                    
                    echo "Finalizado. Enviando resultado ($bStatus) y Ticket ID (${params.JIRA_TICKET_ID}) a GitHub..."
                    
                    bat """
                    "%CURL_EXE%" -X POST -H "Authorization: token %TOKEN%" ^
                    -H "Accept: application/vnd.github.v3+json" ^
                    https://api.github.com/repos/YarkoAlarcon/automatizacionPlaywright01/dispatches ^
                    -d "{\\\"event_type\\\": \\\"jenkins_mission_accomplished\\\", \\\"client_payload\\\": {\\\"status\\\": \\\"${bStatus}\\\", \\\"issue_key\\\": \\\"${params.JIRA_TICKET_ID}\\\"}}"
                    """
                }
            }
            echo "Ciclo en Jenkins terminado para el ticket ${params.JIRA_TICKET_ID}."
        }
    }
}
