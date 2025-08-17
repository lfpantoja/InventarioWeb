import { Routes } from '@angular/router';
import { ListaProductosComponent } from './componentes/productos/lista-productos/lista-productos.component';
import { FormProductoComponent } from './componentes/productos/form-producto/form-producto.component';
import { RegistrarTransaccionComponent } from './componentes/transacciones/registrar-transaccion/registrar-transaccion.component';
import { HistorialTransaccionesComponent } from './componentes/transacciones/historial-transacciones/historial-transacciones.component';

export const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'productos' },

    // Productos
    { path: 'productos', component: ListaProductosComponent, title: 'Productos' },
    { path: 'productos/nuevo', component: FormProductoComponent, title: 'Nuevo producto' },
    { path: 'productos/:id', component: FormProductoComponent, title: 'Editar producto' },

    // Transacciones
    { path: 'transacciones/registrar', component: RegistrarTransaccionComponent, title: 'Registrar transaccion' },
    { path: 'transacciones/historial', component: HistorialTransaccionesComponent, title: 'Historial de transacciones' },

    { path: '**', redirectTo: 'productos' }
];
