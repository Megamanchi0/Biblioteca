import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PerfilService } from '../../services/perfil.service';
import { TokenService } from '../../services/token.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-consultar-perfil',
  templateUrl: './consultar-perfil.component.html',
  styleUrl: './consultar-perfil.component.css'
})
export class ConsultarPerfilComponent {

  form: FormGroup;
  msgError: string = "";
  mostrarError: boolean = false;
  inputsActivos: boolean = false

  constructor(private fb: FormBuilder, private perfilService: PerfilService, private tokenService: TokenService,
    private toastr: ToastrService
  ){
    this.form = this.fb.group({
      Id: 0,
      Correo: ["", [Validators.required, Validators.email]],
      Nombre: ["", Validators.required],
      Apellido: ["", Validators.required],
      Direccion: ["", Validators.required],
      Telefono: ["", Validators.required],
    });
    this.form.disable();
  }

  ngOnInit(){
    this.llenarCampos();
  }

  actualizarPerfil(){
    const datos = {
      Id: this.form.value.Id,
      Nombre: this.form.value.Nombre,
      Apellido: this.form.value.Apellido,
      Telefono: this.form.value.Telefono,
      Direccion: this.form.value.Direccion,
      Correo: this.form.get('Correo')?.value
    }

    this.perfilService.actualizarPerfil(datos).subscribe({
      next: respuesta => {
        this.toastr.success(respuesta.message, "¡Perfil actualizado!");
        this.llenarCampos();
        this.desactivarInputs();
      },
      error: err => {
        this.msgError = "Ocurrió un error al actualizar los datos";
        this.mostrarError = true;
      }
    });
  }

  llenarCampos(){
    const token = this.tokenService.decodificarToken();
    this.perfilService.consultarPerfil(token.id).subscribe({
      next: respuesta => {
        this.form.setValue({
          Id: token.id,
          Correo: token.email,
          Nombre: respuesta.nombre,
          Apellido: respuesta.apellido,
          Direccion: respuesta.direccion,
          Telefono: respuesta.telefono
        });
      },
      error: err => {
        console.log(err);
      }
    });
    
  }

  activarInputs(){
    this.inputsActivos = true;
    this.form.enable();
    this.form.get('Correo')?.disable();
  }

  desactivarInputs(){
    this.inputsActivos = false;
    this.form.disable();
    this.llenarCampos();
  }

}
