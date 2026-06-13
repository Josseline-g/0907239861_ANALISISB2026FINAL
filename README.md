# Historias de Usuario / Requerimientos Funcionales
# Sistema de Gestión de Incidentes de Red - NetGuard GT

ID: RF-1
Nombre: Registrar Incidente
Descripción: El operador tiene la capacidad de registrar incidentes de red indicando título, descripción, severidad y sitio afectado.
Prioridad: Alta

---

ID: RF-2
Nombre: Consultar Incidentes
Descripción: El usuario tiene la capacidad de visualizar el listado de incidentes registrados y consultar su información detallada.
Prioridad: Alta

---

ID: RF-3
Nombre: Asignar Técnico
Descripción: El coordinador tiene la capacidad de asignar técnicos a los incidentes registrados, validando la especialidad requerida y la disponibilidad del técnico.
Prioridad: Alta

---

ID: RF-4
Nombre: Reasignar Incidente
Descripción: El coordinador tiene la capacidad de reasignar un incidente a otro técnico cuando sea necesario para balancear cargas de trabajo o por motivos operativos.
Prioridad: Alta

---

ID: RF-5
Nombre: Actualizar Estado de Incidente
Descripción: El técnico tiene la capacidad de actualizar el estado de un incidente siguiendo el flujo establecido por el negocio.
Prioridad: Alta

---

ID: RF-6
Nombre: Gestionar Técnicos
Descripción: El administrador tiene la capacidad de registrar, modificar y consultar la información de los técnicos especializados.
Prioridad: Media

---

ID: RF-7
Nombre: Gestionar Sitios de Red
Descripción: El administrador tiene la capacidad de registrar y consultar los sitios de red donde pueden ocurrir incidentes.
Prioridad: Media

---

ID: RF-8
Nombre: Consultar Historial de Incidente
Descripción: El usuario tiene la capacidad de consultar el historial de cambios de estado y asignaciones realizadas sobre un incidente.
Prioridad: Alta

---

ID: RF-9
Nombre: Escalar Incidentes Críticos
Descripción: El sistema tiene la capacidad de marcar automáticamente como escalados los incidentes críticos o urgentes que permanezcan sin atención por más de dos horas.
Prioridad: Alta

---

ID: RF-10
Nombre: Generar Reportes de Incidentes
Descripción: El supervisor tiene la capacidad de generar reportes de incidentes por estado, severidad, técnico asignado y cumplimiento de SLA.
Prioridad: Alta




# NetGuard GT

## Tecnologías
- ASP.NET Core Web API
- SQLite
- Entity Framework Core
- NUnit

## Ejecutar

1. Abrir solución.
2. Ejecutar Update-Database.
3. Iniciar proyecto.
4. Abrir Swagger.

## Endpoints

- GET /api/Tecnicos
- POST /api/Tecnicos
- GET /api/Sitios
- POST /api/Sitios
- GET /api/Incidentes
- POST /api/Incidentes
- PUT /api/Incidentes/{id}/asignar/{tecnicoId}
- PUT /api/Incidentes/{id}/reasignar/{tecnicoId}
- PUT /api/Incidentes/{id}/estado

## Reportes

- GET /api/Incidentes/reporte/estado
- GET /api/Incidentes/reporte/severidad
- GET /api/Incidentes/reporte/escalados
- GET /api/Incidentes/reporte/tecnico



## Despliegue en Render

1. Subir el proyecto a GitHub.
2. Crear una cuenta en https://render.com
3. Seleccionar "New Web Service".
4. Conectar el repositorio de GitHub.
5. Configurar:
   - Runtime: Docker o .NET
   - Build Command:
     dotnet publish -c Release
   - Start Command:
     dotnet NetGuardGT.dll
6. Desplegar el servicio.
7. Utilizar la URL pública generada por Render para acceder a la API.
