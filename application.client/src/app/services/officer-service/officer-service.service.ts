import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASEURL, GetToken } from '../../app.config';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OfficerServiceService {

  public uploadUrl = BASEURL+"/Officer/bulk-upload-officer";
  public token = GetToken();
  public headers:any = new HttpHeaders({
      'Authorization': 'Bearer '+ this.token
    });

  constructor(private http:HttpClient) { }

  uploadOfficersData(obj:any):Observable<any>{
    const token = GetToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const options = { headers };
    return this.http.post(this.uploadUrl, obj, options);
  }
}
