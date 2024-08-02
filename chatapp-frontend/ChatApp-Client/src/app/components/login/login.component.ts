import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthResponse } from '../../interfaces/auth-response';
import { LoginRequest } from '../../interfaces/login-request';
import { map, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { TokenResponse } from '../../interfaces/token-response';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
name: string = "";
password: string ="";
private tokenKey = 'token';
private id = 'Id';
private username = 'name';
private refreshToken = 'refreshToken';
private isRefreshing = false;


constructor(private http: HttpClient,
  private router : Router
){
  
}

login(data: LoginRequest): Observable<AuthResponse> {
  const url = `https://localhost:7054/api/Auth/Login`; 
  return this.http.post<AuthResponse>(url, data).pipe(
    map((response) => {
      localStorage.setItem(this.refreshToken,response.refreshToken);
      localStorage.setItem(this.username,response.name);
      localStorage.setItem(this.tokenKey, response.token);
      localStorage.setItem(this.id, JSON.stringify(response));
      return response;
    })
  );
}


UpdateRefreshToken(): Observable<TokenResponse> {
  const refresh = localStorage.getItem('refreshToken');
  return this.http.get<TokenResponse>("https://localhost:7054/api/Auth/RefreshToken?refreshToken=" +refresh).pipe(
    map(response => {
      localStorage.setItem(this.refreshToken,response.refreshToken);
      localStorage.setItem(this.tokenKey,response.token);
      return response;
    })
  );
}

onSubmit() {
  const loginData: LoginRequest = {
    name: this.name,
    password: this.password
  };

  this.login(loginData).subscribe(
    response => {
      console.log('Login successful');
      this.router.navigateByUrl("/");
      // Redirect to another page or perform other actions
    },
    error => {
      console.error('Login failed', error);
    }
  );
}
}