# Configuración de Jira para Automatización

Este documento explica cómo configurar Jira para que funcione con el pipeline de Jenkins.

---

## Crear Campos Personalizados en Jira

### Paso 1: Acceder a la Configuración de Jira
1. Como **administrador**, ve a: **Jira** → **Configuración** → **Campos**
2. Click en **Campos personalizados**
3. Click en **Crear campo personalizado**

### Paso 2: Crear el campo "Test Name"

**Campo 1: Test Name**
- **Nombre:** `Test Name`
- **Tipo:** `Texto (campo de texto)**
- **Descripción:** `Nombre del test a ejecutar (ej: LoginExitoso, UsuarioBloqueado, etc.)`
- **ID del campo:** Anota esto, lo necesitarás (ej: `customfield_10000`)

Una vez creado, el sistema te mostrará el ID. **Guárdalo.**

---

## Configurar Jenkins Credentials

### En Jenkins:

1. Ve a **Jenkins** → **Manage Jenkins** → **Manage Credentials** → **System** → **Global credentials**
2. Click en **+ Add Credentials**
3. Crea 3 credenciales tipo **Secret text**:

| ID | Valor |
|----|-------|
| `jira-url` | `https://tu-jira.com` (sin / al final) |
| `jira-user` | `tu-usuario@email.com` |
| `jira-token` | Tu API token de Jira |

### Cómo obtener el API Token de Jira:
1. En Jira, ve a tu **perfil** → **Configuración**
2. Click en **API tokens**
3. Click en **Crear token**
4. Copia el token y guárdalo en Jenkins

---

## Actualizar el Jenkinsfile (si el ID es diferente)

Si el campo personalizado tiene un ID diferente a `customfield_10000`, debes actualizar el Jenkinsfile:

**Encuentra esta línea:**
```groovy
grep -oP '(?<="customfield_10000":").*?(?=")' | head -1
```

**Reemplázala con tu ID real:**
```groovy
grep -oP '(?<="customfield_XXXXX":").*?(?=")' | head -1
```

Donde `XXXXX` es el ID de tu campo.

---

## Crear un Ticket de Prueba

1. Crea un nuevo ticket en Jira
2. **Tipo:** Bug, Tarea, o el que uses para tests
3. **Rellena el campo "Test Name"** con uno de estos valores:
   - `LoginExitoso`
   - `UsuarioBloqueado`
   - `PasswordIncorrecto`
   - `ValidarCarrito`

4. **Guarda el ticket**

---

## Configurar Webhook en Jira (Opcional pero Recomendado)

Para que Jenkins se dispare automáticamente cuando cambies el estado del ticket:

### En Jira → Automaciones:

1. Ve a **Automaciones** (Jira Automation)
2. Click en **Crear regla**
3. Configurar:
   - **Cuando:** Transición de actividad realizada → **De:** Por hacer → **Para:** En curso
   - **Luego:** Enviar una solicitud web (HTTP POST)
   - **URL:** `http://tu-jenkins:8080/job/SauceDemoLogin/buildWithParameters?JIRA_TICKET_ID={{issue.key}}`
   - **Método:** POST
   - **Headers:** 
     - `Authorization: Bearer [TU_TOKEN_JENKINS]`
     - `Accept: application/json`

---

## Probar manualmente en Jenkins

1. Ve al job en Jenkins: **SauceDemoLogin**
2. Click en **Build Now** (o **Construir ahora**)
3. Aparecerá un formulario pidiendo **JIRA_TICKET_ID**
4. Ingresa: `PLAYW-1` (o el ID de tu ticket de prueba)
5. Click en **Build** (o **Construir**)

El pipeline debería:
- Obtener la información del ticket de Jira
- eer el campo "Test Name"
- Ejecutar el test especificado
- Mostrar resultados

---

## Resumen de Valores a Recordar:

```
JIRA_URL: https://tu-jira.com
JIRA_USER: tu-usuario@email.com
JIRA_TOKEN: [Tu API token]
CAMPO_TEST_NAME_ID: customfield_10000 (o el ID real)
```

---

## Solución de Problemas

### Error: "No se encontró el campo 'Test Name'"
- Verifica que el campo personalizado existe en Jira
- Verifica que el ID `customfield_10000` es correcto
- Revisa que el ticket tiene el campo completado

### Error: "Test no existe"
- Verifica que escribiste correctamente el nombre del test
- Los nombres válidos son: `LoginExitoso`, `UsuarioBloqueado`, `PasswordIncorrecto`, `ValidarCarrito`
- Respeta mayúsculas y minúsculas

### Error de Autenticación en Jira
- Verifica que el API token es válido
- Verifica que el usuario y token están correctos en Jenkins Credentials
- Prueba manualmente: `curl -u usuario:token https://tu-jira.com/rest/api/3/issues/PLAYW-1`

---

**¡Listo! Tu automatización está configurada.**
