import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProductosService } from '../../../core/servicios/productos.service';
import { Producto } from '../../../modelos/producto.model';
import { map } from 'rxjs';

@Component({
  standalone: true,
  selector: 'app-form-producto',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './form-producto.component.html',
  styleUrls: ['./form-producto.component.scss']
})
export class FormProductoComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(ProductosService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id = signal<string | null>(null);

  form = this.fb.group({
    nombre: ['', Validators.required],
    categoria: ['', Validators.required],
    precio: [0, [Validators.required, Validators.min(0)]],
    existencias: [0, [Validators.required, Validators.min(0)]],
    descripcion: [''],
    urlImagen: ['']
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.id.set(id);
      this.api.obtener(id).subscribe(p => this.form.patchValue(p));
    }
  }

  guardar() {
    const data = this.form.value as Producto;
    const id = this.id();

    const obs$ = id
      ? this.api.actualizar(id, data)
      : this.api.crear(data).pipe(map(() => void 0));

    obs$.subscribe(() => this.router.navigate(['/productos']));
  }
}
