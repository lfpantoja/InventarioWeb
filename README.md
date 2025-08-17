# Inventario Web — Frontend (Angular)

## Requisitos

### Resumen de versiones
- **.NET SDK**: 8.0.x
- **Visual Studio**: Community 2022 17.14.12
- **Entity Framework Core**: 9.0.8
- **Npgsql EF Core**: 9.0.8
- **Swashbuckle.AspNetCore**: 9.0.3
- **PostgreSQL**: 17.4
- **Node.js**: 20.19.4
- **npm**: 10.8.2
- **Angular CLI**: 20.1.6

---

### Requisitos del entorno
- **Sistema Operativo**: Windows 11 Pro x64
- **Base de Datos**: PostgreSQL `localhost:5432`
- **Conectividad**: Puertos libres
	- **ProductosAPI**: `http://localhost:5005`
	- **TransaccionesAPI**: `http://localhost:5006`
	- **Frontend Angular**: `http://localhost:4200`

---	

### Requisitos por proyecto
1. **Microservicio: ProductosAPI**
	- **.NET SDK**: 8.0.x
	- **NuGet**: mismas versiones en todos los proyectos .NET
		- `Microsoft.EntityFrameworkCore` 9.0.8
		- `Microsoft.EntityFrameworkCore.Design` 9.0.8
		- `Microsoft.EntityFrameworkCore.Relational` 9.0.8
		- `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.4
		- `Swashbuckle.AspNetCore` 9.0.3
	- **Base de Datos**: PostgreSQL con bases de datos `productosdb` con la tabla `productos`

> **CORS**: las APIs deben permitir el origen `http://localhost:4200`.  
> En .NET de cada API se lo agrega en `Program.cs`:  
> `p.WithOrigins("http://localhost:4200")`  

**Configuración requerida**: (`appsettings.json`)
```json
	{
		"ConnectionStrings": {
			"Default": "Host=localhost;Port=5432;Database=productosdb;Username=postgres;Password=postgres"
		}
	}
```

2. **Microservicio: TransaccionesAPI**
	- **.NET SDK**: 8.0.x
	- **NuGet**: mismas versiones en todos los proyectos .NET
		- `Microsoft.EntityFrameworkCore` 9.0.8
		- `Microsoft.EntityFrameworkCore.Design` 9.0.8
		- `Microsoft.EntityFrameworkCore.Relational` 9.0.8
		- `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.4
		- `Swashbuckle.AspNetCore` 9.0.3
	- **Base de Datos**: PostgreSQL con bases de datos `productosdb` con la tabla `transacciones`

> **CORS**: las APIs deben permitir el origen `http://localhost:4200`.  
> En .NET de cada API se lo agrega en `Program.cs`:  
> `p.WithOrigins("http://localhost:4200")` 

**Configuración requerida**: (`appsettings.json`)
```json
	{
		"ConnectionStrings": {
			"Default": "Host=localhost;Port=5432;Database=productosdb;Username=postgres;Password=postgres"
		},
		"Servicios": {
			"Productos": {
				"BaseUrl": "http://localhost:5005"
			}
		}
	}
```

3. **Frontend: inventario-web (Angular)**
	- **Node.js**: 20.19.4
	- **npm**: 10.8.2
	- **Angular CLI**: 20.1.6

**Configuración requerida**: (`src/environments/environment.ts`)
```ts
	export const environment = {
		production: false,
		productosApi: 'http://localhost:5005',
		transaccionesApi: 'http://localhost:5006' 
	};
```

> [!IMPORTANT]
> Clonar el proyecto
> -	Base de Datos (PostgreSQL)
> 	- Crear Base de Datos `productosdb`
> 	- Ejecutar los scripts SQL de productos y transacciones.
> 	- Verificar acceso: (Host, Puerto, Usuario, Contraseña).

---

## Ejecución del backend (local - Visual Studio 2022)

### Backend Productos

1. Buscamos en el directorio `ServicioProducto` el archivo `ProductosAPI.sln`
```
ServicioProducto/
├── ProductosAPI/
│   └── ProductosAPI.sln
├── ProductosAplicacion/
├── ProductosDominio/
└── ProductosInfraestructura/
```

2. En el Explorador de la solución buscamos el archivo `launchSettings.json` para buscar el siguiente código donde cambiaremos el puerto.
```
"applicationUrl": "http://localhost:5005"
```

3. Iniciar depuración con la tecla F5.

### Backend Transacciones

1. Buscamos en el directorio `ServicioTransacciones` el archivo `TransaccionesAPI.sln`
```
ServicioTransacciones/
├── TransaccionesAPI/
│   └── TransaccionesAPI.sln
├── TransaccionesAplicacion/
├── TransaccionesDominio/
└── TransaccionesInfraestructura/
```

2. En el Explorador de la solución buscamos el archivo `launchSettings.json` para buscar el siguiente código donde cambiaremos el puerto.
```
"applicationUrl": "http://localhost:5006"
```

3. Iniciar depuración con la tecla F5.

---

## Ejecución del frontend (local)

1) **Entrar** al proyecto
```bash
cd inventario-web
```

2) **Instalar dependencias**
```bash
npm install
```

3) **Configurar ambientes**  
Edita `src/environments/environment.ts` Y `environment.prod.ts`:

```ts
export const environment = {
  production: false,
  productosApi: 'http://localhost:5005',
  transaccionesApi: 'http://localhost:5006'
};
```

4) **Arrancar en modo desarrollo**
```bash
ng serve -o
```
En caso de requerir abrir con la IP del dispositivo se agrega --host x.x.x.x
```bash
ng serve -o --host x.x.x.x
```
---

## Evidencias

- Listado dinámico de productos y transacciones con paginación
![Listado dinámico de productos con paginación.](https://github.com/lfpantoja/InventarioWeb/blob/main/imagenes/productos-listado-paginacion.png)
- Pantalla para la creación de productos
- Pantalla para la edición de productos
- Pantalla para la creación de transacciones
- Pantalla para la edición de transacciones.
- Pantalla de filtros dinámicos.
- Pantalla para la consulta de información de un formulario (extra).

---

## Notas útiles

- **Scripts recomendados** (en `package.json`):
  ```json
  {
    "scripts": {
      "start": "ng serve",
      "build": "ng build",
    }
  }
  ```

build para generar el dist
serve para depurar el proyecto

---

## Endpoints esperados por el frontend

- Productos:
  - `GET /api/productos`
  - `GET /api/productos/{id}`
  - `POST /api/productos`
  - `PUT /api/productos/{id}`
  - `DELETE /api/productos/{id}`
  - `POST /api/productos/{id}/ajustar-existencias`

- Transacciones:
  - `POST /api/transacciones`
  - `GET /api/transacciones`
  - `GET /api/transacciones/historial`

---