import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PerfilService } from '../../services/perfil.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registro-perfil',
  templateUrl: './registro-perfil.component.html',
  styleUrl: './registro-perfil.component.css'
})
export class RegistroPerfilComponent {
  form: FormGroup;
  msgError: string = "El correo ingresado ya está registrado";
  mostrarError: boolean = false;

  constructor(private fb: FormBuilder, private servicioRegistro: PerfilService, private toastr: ToastrService, private router: Router){
    this.form = this.fb.group({
      Correo: ["", [Validators.required, Validators.email]],
      Contrasena: ["", Validators.required],
      Nombre: ["", Validators.required],
      Apellido: ["", Validators.required],
      Direccion: ["", Validators.required],
      Telefono: ["", Validators.required]
    })
  }

  registrarse(){
    this.servicioRegistro.registrarse(this.form.value).subscribe({
      next: respuesta => {
        this.toastr.success(respuesta.message, "¡Registro exitoso!");
        this.router.navigate(['/login']);
      },
      error: err =>{
        if (err.status==409) {
          this.mostrarError = true;
        }
        this.toastr.error("Hubo un error al realizar el registro", "Error")
      }
    });
  }
}
