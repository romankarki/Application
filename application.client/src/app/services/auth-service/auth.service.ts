import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BASEURL } from '../../app.config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  public baseURL : string = BASEURL;
  public authUrl = this.baseURL + "/Officer/authenticate-officer";

  AuthenticateOfficer(obj:any):Observable<any>{
    return this.http.post<any>(this.authUrl,obj);
  }
}
