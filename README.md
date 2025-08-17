# Inventario Web — Frontend (Angular)

## Requisitos

### Resumen de versiones
- **.NET SDK**: 8.0.x
- **Visual Studio**: Community 2022 17.14.12
- **Entity Framework Core**: 9.0.4
- **Npgsql EF Core**: 9.0.4
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
	- **Base de Datos**: PostgreSQL con bases de datos `productosdb` con tablas `productos` y `transacciones`

> **CORS**: las APIs deben permitir el origen `http://localhost:4200`.  
> En .NET de cada API se lo agrega en `Program.cs`:  
> `p.WithOrigins("http://localhost:4200")`  

> **Configuración requerida**: (`appsettings.json`)
> {
>   "ConnectionStrings": {
>   	"Default": "Host=localhost;Port=5432;Database=productosdb;Username=postgres;Password=postgres"
> 	}
> }


---

## Ejecución del backend (local)

---

## Ejecución del frontend (local)

1) **Clonar** y entrar al proyecto
```bash
git clone <URL_DEL_REPOSITORIO>
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
Build para generar el dist
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