- Instalacion de framework .Net core
- extencion de **C#** para desarrollo en VScode

- ejecucion en la terminal del proyecto: **dotnet new mvc -n Inmobiliaria** (inicializa el proyecto y crea la estructura base)

-ejecucion en la terminal del proyecto:
**dotnet add package Microsoft.EntityFrameworkCore.SqlServer** (para la conexion a SQL server)**NO**
**dotnet add package Microsoft.EntityFrameworkCore.Tools**(tools para las migraciones)
**dotnet add package Pomelo.EntityFrameworkCore.MySql** (EF CORE para Mysql(pomelo), ya que usamos xampp)

- configuracion de archivo de conexion en appsettings.json:

- creacion de la clase de contexto "DbContext" dentro de Models, en el archivo ApplicationDbContext
aqui van las entidades de la base de datos que mas adelante migraremos(propietarios e inquilinos)

- inquilino(nombre, apellido, dni, email, teléfono)
- propietario(nombre, apellido, dni, email, télefono)

- en program.cs registre el contexto(conexion con Mysql)

- agregado de paquete de herramientas scalffolding para codeGenerator:
 **dotnet tool install -g dotnet-aspnet-codegenerator**

- agregar paquetes necesarios para el proyecto:
**dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design**
**dotnet add package Microsoft.EntityFrameworkCore.Design**

- ejecutar scalffolding para inquilino y propietario:
propietario
**dotnet aspnet-codegenerator controller -name PropietariosController -m Propietario -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --force**

**dotnet aspnet-codegenerator controller -name InquilinosController -m Inquilino -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --forced**




**--------------------------------------------------------------------------------------------------------------------------------------**
## 🚀 Tecnologías utilizadas
- **C# .NET 8** (ASP.NET MVC)
- **ADO.NET** con `MySqlConnector` (sin Entity Framework)
- **MySQL** como base de datos
- **Bootstrap 5** para el diseño de las vistas
- Arquitectura **MVC** (Modelos, Vistas, Controladores)

# En una primera etapa el proyecto fue desarrollado utilizando Entity Framework Core, con el que se implementaron dos módulos de ABM.
# Posteriormente se cambio a ado.net por requerimiento academico, y mis disculpas por no prestar atenciony hacerlo asi del principio

**PARA REALIZAR ESTE CAMBIO FUE NECESARIO**

- Eliminar todas las declaraciones y configuraciones asociadas a Entity Framework Core.

- Reestructurar los modelos de datos y la capa de acceso a datos para trabajar directamente con ADO.NET (SqlConnection, SqlCommand,         SqlDataReader, etc.).

- Reescribir los ABM ya implementados y continuar con el resto de funcionalidades bajo el nuevo esquema.

## 🔑 Funcionalidades principales
- Gestión de **Inmuebles** (ABM).
- Gestión de **Inquilinos** (ABM).
- Gestión de **Contratos** (ABM).
- Gestión de **Pagos** asociados a los contratos(ABM). parcial(proxima entrega terminado)
- Validaciones de formularios en servidor y cliente.
- Interfaz intuitiva y responsive con Bootstrap.


Proximas mejoras a implementar

Inmueble:
atibutos uso-tipo, hacerlos enum y usar
uso: habitacional, comercial, industrail, agropecuario
tipo: terreno, construccion, residencial

Atributo Superficie, avisar que es en m2

