# AGENTS.md — SolutionComponents (repo de código)

Repo de código de «Solution Component Inspector». Se orquesta desde su meta-repo (`solution-components-agents`). Aquí solo vive el código de la aplicación.

## Comandos (literales, probados; los usa el método en cada cierre)

| Para… | Comando |
|---|---|
| Levantar el entorno desde cero | `& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" SolutionComponents.sln -t:restore` |
| Correr la suite completa (build y tests) | `& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" SolutionComponents.sln` |
| Correr los linters y análisis estático | `& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" SolutionComponents.sln /p:WarningLevel=4` |
| Lanzar una instancia para que la use el usuario | Cargar `bin\Debug\Plugins\SolutionComponents.dll` en XrmToolBox |
| Comprobación de seguridad | `& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" SolutionComponents.sln` |
| Parar / limpiar | `& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" SolutionComponents.sln -t:Clean` |

## Qué necesita esta máquina

- **.NET Framework 4.8 / 4.8.1 SDK & Targeting Pack**.
- **Visual Studio 2022 con MSBuild** (`C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`).
- **Python 3.12** para scripts de orquestación del meta-repo.
- **Git** versión 2.40+.

## Estructura

- `SolutionComponents.sln` / `SolutionComponents.csproj`: Solución y proyecto de Visual Studio configurados para .NET Framework 4.8 (`net48`).
- `MyPlugin.cs`: Clase principal del plugin con metadatos MEF para integración con XrmToolBox (`IXrmToolBoxPlugin`).
- `MyPluginControl.cs` / `MyPluginControl.designer.cs`: Control de usuario WinForms (`PluginControlBase`) que implementa la interfaz visual y la interacción con Dataverse.
- `Settings.cs`: Configuración persistente del plugin (URL de organización, filtros).
- `packages.config`: Dependencias NuGet gestionadas localmente en `packages/`.

## Reglas de este repo

- Las consultas a Dataverse deben ejecutarse asíncronamente mediante `WorkAsync` para no congelar la interfaz WinForms.
- No se suben secretos ni credenciales; la conexión la gestiona el host de XrmToolBox.
- Todo cambio de comportamiento debe compilar con 0 errores y verificarse localmente antes del merge.
