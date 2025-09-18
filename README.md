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

