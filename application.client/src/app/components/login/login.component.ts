import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private router:Router,private authService:AuthService){

  }
  public identificationNumber : string = "";
  public password : string = "";
  public onSubmit(){
    let data = {
      "identificationNumber": this.identificationNumber,
      "password": this.password
    }
      this.authService.AuthenticateOfficer(data).subscribe({
        next: (res)=>{
        console.log("The response from login is", res);
        let userInfo = res;
        let stringInfo = JSON.stringify(res);
        sessionStorage.setItem("user", stringInfo);
        this.router.navigate(['/']); 
      },
      error: (error)=>{
        alert("Failed To Authenticate, Please Try again!")
      }
      });
  }


}
