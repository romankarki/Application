import { Component, OnInit } from '@angular/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { FacilityService } from '../../services/facility-service/facility.service';

@Component({
  selector: 'app-facility',
  standalone: true,
  imports: [NgxDatatableModule],
  templateUrl: './facility.component.html',
  styleUrl: './facility.component.css'
})
export class FacilityComponent implements OnInit {
  public loadingIndicator = false;
  public page = {size:20, number: 1}

  constructor(private facilityService:FacilityService){}

  ngOnInit(): void {
    
  }
  

  rows = [
    { name: 'Austin', gender: 'Male', company: 'Swimlane' },
    { name: 'Dany', gender: 'Male', company: 'KFC' },
    { name: 'Molly', gender: 'Female', company: 'Burger King' }
  ];
  public fileSelect(event:any){

  }

  public setPage(event:any){

  }

}
